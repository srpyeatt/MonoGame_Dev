using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine.Components;

namespace CPI311.GameEngine.Rendering
{
    public class Light : Component
    {
        public static Light Current { get; set; }
        public static Light Default { get; set; }

        public Color Ambient { get; set; }
        public Color Diffuse { get; set; }
        public Color Specular { get; set; }

        public Light()
        {
            Ambient = Color.White;
            Diffuse = Color.White;
            Specular = Color.White;
        }

        static Light()
        {
            Default = new Light();
            Default.Transform = new Transform();
            Current = Default;
        }
    }
}
