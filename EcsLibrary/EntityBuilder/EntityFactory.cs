using System.IO;
using EcsLibrary.Components;
using Microsoft.Xna.Framework.Content;

namespace EcsLibrary
{
    public class EntityFactory
    {
        private static ContentManager _contentManager;

        public static void Setup(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        // public static TacticalRoguelike.TacticalRoguelikeEntityData LoadEntity(string entityRef)
        // {
        //     using (var r = _contentManager.OpenStream(entityRef + ".json"))
        //     {
        //         using (var st = new StreamReader(r))
        //         {
        //             JsonReader jsonReader = new JsonTextReader(st);
        //             JsonSerializer jsonSerializer = new JsonSerializer();
        //             return jsonSerializer.Deserialize<TacticalRoguelike.TacticalRoguelikeEntityData>(jsonReader);
        //         }
        //     }
        //     //return _contentManager.Load<EntityData>(entityRef);
        // }

        // public static void BuildEntity(string entityRef, ref Entity entity)
        // {
        //     BuildEntity(LoadEntity(entityRef), ref entity);
        // }
        //
        // public static void BuildEntity(TacticalRoguelike.TacticalRoguelikeEntityData data, ref Entity entity)
        // {
        //     foreach (var dataComponent in data.components)
        //     {
        //         entity.AddComponent(dataComponent.GetComponent());
        //     }
        //
        //     TransformComponent trans = new TransformComponent();
        //     if (entity.HasComponent<TransformComponent>())
        //     {
        //         trans= entity.GetComponent<TransformComponent>();
        //     }
        //     
        //     if (data.subEntities == null)
        //         return;
        //     foreach (var dataSubEntity in data.subEntities)
        //     {
        //         Entity newSub;
        //         if (dataSubEntity.entityRef != null)
        //         {
        //             newSub = entity.AddSubEntity();
        //             BuildEntity(LoadEntity(dataSubEntity.entityRef), ref newSub);
        //             foreach (var component in dataSubEntity.values)
        //             {
        //                 newSub.OverrideValues(component.GetComponent());
        //             }
        //         }
        //         else
        //         {
        //             newSub = entity.AddSubEntity();
        //             foreach (var dataComponent in dataSubEntity.values)
        //             {
        //                 newSub.AddComponent(dataComponent.GetComponent());
        //             }
        //         }
        //         if (newSub.HasComponent<TransformComponent>())
        //         {
        //             var subTrans = newSub.GetComponent<TransformComponent>();
        //             subTrans.x += trans.x;
        //             subTrans.y += trans.y;
        //             subTrans.scaleX *= trans.scaleX;
        //             subTrans.scaleY *= trans.scaleY;
        //         }
        //     }
        // }
    }
}