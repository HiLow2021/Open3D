using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Open3D.Shader;
using Open3D.Demo.Tester;

namespace Open3D.Demo
{
    public partial class Form1 : Form
    {
        private IShaderProgram coloredSolidShader;
        private IShaderProgram texturedSolidShader;
        private TesterBase tester;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            coloredSolidShader = new ShaderProgram(new Dictionary<ShaderType, string>
            {
                { ShaderType.VertexShader, @"Shaders\45\coloredVertexShader.vert.txt" },
                { ShaderType.FragmentShader, @"Shaders\45\coloredFragmentShader.frag.txt" }
            }, 20, 21, 22);
            texturedSolidShader = new ShaderProgram(new Dictionary<ShaderType, string>
            {
                { ShaderType.VertexShader, @"Shaders\45\texturedVertexShader.vert.txt" },
                { ShaderType.FragmentShader, @"Shaders\45\texturedFragmentShader.frag.txt" }
            }, 20, 21, 22);

            tester = new PlanetTester(open3DViewer1, coloredSolidShader, texturedSolidShader);
        }
    }
}
