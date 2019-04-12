using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.AttributeMetadata;
using Autofac.Features.AttributeFilters;

namespace Lec49_AttributeBasedMetadata
{
    [MetadataAttribute]
    public class AgeMetadataAttribute : Attribute
    {
        public int Age { get; set; }

        public AgeMetadataAttribute(int age)
        {
            Age = age;
        }
    }

    public interface IArtwork
    {
        void Display();
    }

    [AgeMetadata(1000)]
    public class MillenialArtwork : IArtwork
    {
        public void Display()
        {
            Console.WriteLine("Displaying a Really old piece of art");

        }
    }

    [AgeMetadata(100)]
    public class CenturyArtwork : IArtwork
    {
        public void Display()
        {
            Console.WriteLine("Displaying a century-old piece");
        }
    }

    public class ArtDisplay
    {
        private IArtwork artwork;

        public ArtDisplay([MetadataFilter("Age", 100)]IArtwork artwork)
        {
            this.artwork = artwork ?? throw new ArgumentNullException(nameof(artwork));
        }

        public void Display()
        {
            artwork.Display();
        }
    }

    /*
    # Attribute Based Metadata
     - need extra packages
     - Autofac.Extra.AttributeMetadata

    */
    class Program
    {
        static void Main(string[] args)
        {
            var b = new ContainerBuilder();
            b.RegisterModule<AttributedMetadataModule>();
            b.RegisterType<CenturyArtwork>().As<IArtwork>();
            b.RegisterType<MillenialArtwork>().As<IArtwork>();
            b.RegisterType<ArtDisplay>().WithAttributeFiltering();

            using (var c = b.Build())
            {
                c.Resolve<ArtDisplay>().Display();
            }

            Console.ReadKey();
        }
    }
}
