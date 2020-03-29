using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.BL.Entities
{
    public class Image : IImage
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
