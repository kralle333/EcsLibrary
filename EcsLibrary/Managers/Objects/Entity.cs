using System.Collections.Generic;

namespace EcsLibrary.Managers.Objects
{
    public struct Entity
    {
        public int Id { get; }
        public Aspect Aspect { get; private set; }

        public Entity(int id)
        {
            Id = id;
            Aspect = new Aspect();
        }
        
        public void SetAspect(Aspect aspect)
        {
            Aspect = aspect;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Entity entity)
            {
                return entity.Id == Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return $"[Entity][{Id.ToString()}]";
        }
    }
}