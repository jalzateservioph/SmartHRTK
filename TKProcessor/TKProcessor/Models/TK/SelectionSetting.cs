using System;

namespace TKProcessor.Models.TK
{
    public class SelectionSetting : IModel
    {
        public SelectionSetting()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
