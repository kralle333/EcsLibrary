using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace EcsLibrary.Components
{
    [Serializable]
    public class AnimatedTexture2DComponent : RenderableComponent
    {
        private string _texturePath;
        private TextureInitInfo[] _textureInitInfos = new TextureInitInfo[0];
        private PlaybackInitInfo[] _playbackInitInfos = new PlaybackInitInfo[0];
        private string _startAnimation;
        private int _width;
        private int _height;

        public struct TextureInitInfo
        {
            public string Name;
            public int StartIndexX;
            public int EndIndexX;
            public int StartIndexY;
            public int Border;
        }

        public class PlaybackInitInfo
        {
            public string Name;
            public string FramesName;
            public int[] Steps;
            public float PlaybackSpeed;
        }
        
        private Texture2D _texture;
        private Dictionary<string, Rectangle[]> _frameData = new Dictionary<string, Rectangle[]>();
        private Dictionary<string, AnimationData> _animationSteps = new Dictionary<string, AnimationData>();

        private struct AnimationData
        {
            public int[] Steps;
            public string FramesName;
            public float PlayBackSpeed;
        }
        private int _currentAnimationStep;
        private string _playedAnimation;

        public AnimatedTexture2DComponent(string texturePath, int width, int height)
        {
            _texturePath = texturePath;
            _width = width;
            _height = height;
        }

        public AnimatedTexture2DComponent Play(string animation)
        {
            _playedAnimation = animation;
            return this;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            if (_textureInitInfos.Length > 0)
            {
                _frameData.Clear();
                foreach (TextureInitInfo info in _textureInitInfos)
                {
                    AddHorizontalFrames(info.Name, info.StartIndexX, info.EndIndexX, info.StartIndexY, info.Border);
                }
                _textureInitInfos = Array.Empty<TextureInitInfo>();
            }

            if (_playbackInitInfos.Length > 0)
            {
                _animationSteps.Clear();
                foreach (PlaybackInitInfo info in _playbackInitInfos)
                {
                    AddPlaybackInfo(info.Name, info.FramesName, info.Steps, info.PlaybackSpeed);
                }
                _playbackInitInfos = new PlaybackInitInfo[0];
            }

            _texture = contentManager.Load<Texture2D>(_texturePath);
            if (_startAnimation != null)
            {
                Play(_startAnimation);
            }
        }

        public float AnimationSpeed()
        {
            return _animationSteps[_playedAnimation].PlayBackSpeed;
        }
        public void NextStep()
        {
            _currentAnimationStep = (_currentAnimationStep + 1) % _animationSteps[_playedAnimation].Steps.Length;
        }

        public AnimatedTexture2DComponent AddHorizontalFrames(string name, int startIndexX, int endIndexX, int startIndexY, int border = 0)
        {
            Debug.Assert(endIndexX >= startIndexX);
            Debug.Assert(startIndexX >= 0);
            Debug.Assert(startIndexY >= 0);

            var frames = new Rectangle[endIndexX - startIndexX];
            var y = startIndexY * _height;
            for (int i = startIndexX, f = 0; i < endIndexX; i++, f++)
            {
                frames[f] = new Rectangle(i * _width, y, _width, _height);
            }
            _frameData[name] = frames;
            return this;
        }
        public AnimatedTexture2DComponent AddPlaybackInfo(string name,string framesName, int[] steps, float playbackSpeed)
        {
            _animationSteps[name] = new AnimationData()
            {
                Steps = steps,
                FramesName = framesName,
                PlayBackSpeed = playbackSpeed
            };
            return this;
        }

        private Rectangle GetRectangle()
        {
            var shownFrame = _animationSteps[_playedAnimation].Steps[_currentAnimationStep];
            return _frameData[_playedAnimation][shownFrame];
        }

        public void Draw(SpriteBatch spriteBatch, TransformComponent transform)
        {
            spriteBatch.Draw(_texture, transform.Pos, GetRectangle(), Color.White, transform.Rotation, transform.Origin, transform.Scale, SpriteEffects.None, 1);
        }
    }
}
