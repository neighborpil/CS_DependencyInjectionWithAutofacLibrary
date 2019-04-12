using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.AttributeMetadata;
using Autofac.Features.AttributeFilters;

namespace Test_AttribueBasedMetadata
{
    [MetadataAttribute]
    public class GenderMetadataAttribute : Attribute
    {
        public bool Gender { get; set; }

        public GenderMetadataAttribute(bool gender)
        {
            Gender = gender;
        }
    }

    public interface IArtwork
    {
        void Display();
    }

    [GenderMetadata(true)]
    public class CenturyArtwork : IArtwork
    {
        public void Display()
        {
            Console.WriteLine("Century Artwork");
        }
    }

    [GenderMetadata(false)]
    public class MillenniumArtwork : IArtwork
    {
        public void Display()
        {
            Console.WriteLine("Millennial artwork");
        }
    }

    public class ArtDisplay
    {
        private IArtwork artwork;

        public ArtDisplay([MetadataFilter("Gender", true)]IArtwork artwork)
        {
            this.artwork = artwork ?? throw new ArgumentNullException(nameof(artwork));
        }
        public void Display()
        {
            artwork.Display();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var b = new ContainerBuilder();
            b.RegisterModule<AttributedMetadataModule>();
            b.RegisterType<CenturyArtwork>().As<IArtwork>();
            b.RegisterType<MillenniumArtwork>().As<IArtwork>();
            b.RegisterType<ArtDisplay>().WithAttributeFiltering();

            using (var c = b.Build())
            {
                c.Resolve<ArtDisplay>().Display();
            }
            Console.ReadKey();
        }
    }
}
