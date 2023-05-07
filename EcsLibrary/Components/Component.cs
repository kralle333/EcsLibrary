using System;
using Microsoft.Xna.Framework.Content;

namespace EcsLibrary.Components
{
    [Serializable]
    public class Component
    {
        public void Dir()
        {
        }

        public static int TypeId { get; private set; }

        public static void SetTypeId(int typeId)
        {
            TypeId = typeId;
        }
        public virtual void Dispose(){}
        public virtual void LoadContent(ContentManager contentManager)
        {
            
        }
        public virtual void OverrideValues(Component other)
        {
            throw new Exception("Missing override of this type!");
        }
    }
}