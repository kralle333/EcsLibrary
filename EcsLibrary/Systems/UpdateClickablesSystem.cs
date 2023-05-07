using System.Collections.Generic;
using EcsLibrary.Components;
using EcsLibrary.Managers.Objects;
using Microsoft.Xna.Framework;

namespace EcsLibrary.Systems
{
    public class UpdateClickablesSystem : UpdateSystem
    {
        private MouseStateComponent _mouseStateComponent;
        private OrthographicCamera _orthographicCamera;

        public UpdateClickablesSystem(MouseStateComponent mouseStateComponent, OrthographicCamera camera) : base()
        {
            _mouseStateComponent = mouseStateComponent;
            _orthographicCamera = camera;
        }

        protected override void SetRequiredTypes()
        {
            base.SetRequiredTypes();
            SetRequiredTypes(typeof(ClickableComponent), typeof(TransformComponent));
        }

        protected override void UpdateEntities(List<Entity> updatedEntities, GameTime gameTime)
        {
            if (!_mouseStateComponent.LeftClicked())
            {
                updatedEntities.ForEach(x => GetComponent<ClickableComponent>(x).isClicked = false);
                return;
            }

            var mousePos =
                _orthographicCamera.ScreenToWorld(_mouseStateComponent.currentMouseState.Position.ToVector2());

            foreach (Entity entity in updatedEntities)
            {
                var clickable = GetComponent<ClickableComponent>(entity);
                var trans = GetComponent<TransformComponent>(entity);
                clickable.rectangle = new Rectangle(
                    (int)trans.X,
                    (int)trans.Y,
                    clickable.rectangle.Width,
                    clickable.rectangle.Height);
                if (clickable.rectangle.Contains(mousePos))
                {
                    clickable.isClicked = true;
                }
            }
        }
    }
}