using System;
using System.ComponentModel.DataAnnotations;

namespace ODataGeneric.SampleControllers.Entities
{
    /// <summary> Abstract Base Class for all Entities </summary>
    public abstract class AEntity<TKey>
    {

        protected AEntity() { }

        protected AEntity(TKey id, string changeUser){
            Id = id;
            ChangeUser = changeUser;
        }

        [Key]
        public TKey Id { get; set; } = default!;

        [Timestamp]
        public byte[]? Version { get; set; }

        public string ChangeUser { get; set; } = default!;

        public DateTimeOffset ChangeTime { get; set; }

        public bool IsDeleted { get; set; }

    }
}
