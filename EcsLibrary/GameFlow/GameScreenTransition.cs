using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibrary.GameFlow;

public abstract class GameScreenTransition
{
    protected enum TransitionState
    {
        NotStarted,
        Transitioning,
        Finished,
    }

    protected GameScreen _gameScreen;
    private double _transitionTime;
    private double _timer;

    protected TransitionState _state = TransitionState.NotStarted;
    private readonly Action<GameScreen> _onFinishAction;

    public GameScreenTransition(Action<GameScreen> onFinishAction)
    {
        _onFinishAction = onFinishAction;
    }

    public virtual void StartTransition(GameScreen gameScreen, float transitionTime)
    {
        _gameScreen = gameScreen;
        _transitionTime = transitionTime;
        _state = TransitionState.Transitioning;
        _timer = 0;
    }

    public bool UpdateTimer(GameTime gameTime)
    {
        _timer += gameTime.ElapsedGameTime.TotalSeconds;
        return _timer >= _transitionTime;
    }

    private float Delta()
    {
        return (float)Math.Min(1f, _timer / _transitionTime);
    }

    public virtual bool Update(GameTime gameTime)
    {
        switch (_state)
        {
            case TransitionState.Transitioning:
                UpdateTransitioning(Delta());
                if (UpdateTimer(gameTime))
                {
                    _state = TransitionState.Finished;
                    UpdateTransitioning(1);
                    _onFinishAction?.Invoke(_gameScreen);
                    return true;
                }

                break;
            case TransitionState.Finished:
                return true;
        }

        return false;
    }

    protected virtual void UpdateTransitioning(float delta)
    {
    }

    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
    }
}

public class FadingGameScreenTransition : GameScreenTransition
{
    private Texture2D _whiteTexture;
    private Color _fadeColor = Color.Black;
    protected float CurrentAlpha = 0;

    public FadingGameScreenTransition(Texture2D whiteTexture, Action<GameScreen> onFinishAction) : base(onFinishAction)
    {
        _whiteTexture = whiteTexture;
    }

    public void SetFadeColor(Color fadeColor)
    {
        _fadeColor = fadeColor;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(_whiteTexture, new Vector2(0, 0), new Color(_fadeColor.R, _fadeColor.G, _fadeColor.B, CurrentAlpha));
        _gameScreen.Draw(gameTime);
    }
}

public class FadeInGameScreenTransition : FadingGameScreenTransition
{
    public FadeInGameScreenTransition(Texture2D whiteTexture, Action<GameScreen> onFinishAction) : base(whiteTexture, onFinishAction)
    {
    }

    public override void StartTransition(GameScreen gameScreen, float transitionTime)
    {
        base.StartTransition(gameScreen, transitionTime);
        CurrentAlpha = 1;
    }

    protected override void UpdateTransitioning(float delta)
    {
        CurrentAlpha = 1-delta;
    }
    
}

public class FadeOutGameScreenTransition : FadingGameScreenTransition
{
    public FadeOutGameScreenTransition(Texture2D whiteTexture, Action<GameScreen> onFinishAction) : base(whiteTexture, onFinishAction)
    {
    }
    public override void StartTransition(GameScreen gameScreen, float transitionTime)
    {
        base.StartTransition(gameScreen, transitionTime);
        CurrentAlpha = 0;
    }

    protected override void UpdateTransitioning(float delta)
    {
        CurrentAlpha = delta;
    }
}