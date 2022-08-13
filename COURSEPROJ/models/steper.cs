using System.Drawing;
using COURSEPROJ.entities;

namespace COURSEPROJ.models
{
    public interface steper
    {
        float x { get; set; }
        float y { get; set; }
        short ID { get; }
        void step(ref int[,] MAP, ref Hero hero);
        Image img { get; }
        short sub { get; set; }
    }
}