using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace progr
{
   

    public partial class Form1 : Form
    { 
      
        List<Particle> particles = new List<Particle>();
        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter; // добавим поле для эмиттера
        public Form1()
        {
            InitializeComponent();
            // привязал изображение
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            this.emitter = new Emitter // создаю эмиттер и привязываю его к полю emitter
            {
                Direction = 0,
                Spreading = 70,
                SpeedMin = 10,
                SpeedMax = 10,
                ColorFrom = Color.White,
                ColorTo = Color.FromArgb(0, Color.Magenta),
                ParticlesPerTick = 10,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
            };

            emitters.Add(this.emitter); // все равно добавляю в список emitters, чтобы он рендерился и обновлялся
        }


        private void picDisplay_Click(object sender, EventArgs e)
        {

        }

       
        private void timer1_Tick(object sender, EventArgs e) // обработка тика таймера
        {
            emitter.UpdateState(); // каждый тик обновляем систему

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Wheat);
                emitter.Render(g); // рендерим систему через эмиттер
            }

            picDisplay.Invalidate();
        }

        private void tbSpeed_Scroll(object sender, EventArgs e)
        {
            emitter.Speed = tbSpeed.Value / 100.0f;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
        }

        private void tbRadius_Scroll(object sender, EventArgs e)
        {
            emitter.Radius = tbRadius.Value;
        }
    }
}
