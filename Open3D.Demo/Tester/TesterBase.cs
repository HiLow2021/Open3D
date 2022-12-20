using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Open3D.Controls;
using Open3D.Shader;

namespace Open3D.Demo.Tester
{
    public abstract class TesterBase
    {
        protected readonly Timer timer = new Timer();
        protected readonly Open3DViewer open3DViewer;
        protected readonly TestModelFactory testModelFactory;
        protected readonly Random rnd = new Random();

        public TesterBase(Open3DViewer open3DViewer, IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader)
        {
            this.open3DViewer = open3DViewer;
            testModelFactory = new TestModelFactory(coloredSolidShader, texturedSolidShader);
            timer.Interval = 10;
        }

        public void Start() => timer.Start();

        public void Stop() => timer.Stop();
    }
}
