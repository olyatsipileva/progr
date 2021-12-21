using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace progr
{
    public class Emitter
    {
        List<Particle> particles = new List<Particle>();
        
        public float GravitationX = 0;
        public float GravitationY = 0.7f; // пусть гравитация будет силой один пиксель за такт, нам хватит
        public int X; // координата X центра эмиттера, будем ее использовать вместо MousePositionX
        public int Y; // соответствующая координата Y 
        public int Direction = 0; // вектор направления в градусах куда сыпет эмиттер
        public int Spreading = 360; // разброс частиц относительно Direction
        public float Speed = 0.05f;
        public int SpeedMin = 10; // начальная минимальная скорость движения частицы
        public int SpeedMax = 20; // начальная максимальная скорость движения частицы
        public int RadiusMin = 2; // минимальный радиус частицы
        public int RadiusMax = 10; // максимальный радиус частицы
        public int LifeMin = 20; // минимальное время жизни частицы
        public int LifeMax = 100; // максимальное время жизни частицы
        public int ParticlesPerTick = 1;
        public Color ColorFrom = Color.White; // начальный цвет частицы
        public Color ColorTo = Color.FromArgb(0, Color.Magenta); // конечный цвет частиц
        public int Radius = 80;
        public int ParticlesCount = 500;
        public float Time = 0;
        // добавил новый метод, виртуальным, чтобы переопределять можно было
        public virtual Particle CreateParticle()
        {
            var particle = new ParticleColorful();
            particle.FromColor = ColorFrom;
            particle.ToColor = ColorTo;

            return particle;
        }

        public virtual void ResetParticle(Particle particle)
        { 
            // восстанавливаю здоровье
            particle.Life = 20 + Particle.rand.Next(LifeMin, LifeMax);
            // новое начальное расположение частицы — это то, куда указывает курсор
            particle.X = X;
            particle.Y = Y;
            // сброс состояния частицы 
            var direction = Direction
                 + (double)Particle.rand.Next(Spreading)
                 - Spreading / 2;

            var speed = Particle.rand.Next(SpeedMin, SpeedMax);

            particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
            particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

            particle.Radius = Particle.rand.Next(RadiusMin, RadiusMax);
        }
        public void UpdateState()
        {
            int particlesToCreate = ParticlesPerTick; // фиксируем счетчик сколько частиц нам создавать за тик

            foreach (var particle in particles)
            {
                                     
                if (particle.Life < 0)// если здоровье кончилось
                {
                    ResetParticle(particle);

                    if (particlesToCreate > 0)
                    {
                        /* у нас как сброс частицы равносилен созданию частицы */
                        particlesToCreate -= 1; // поэтому уменьшаем счётчик созданных частиц на 1
                        ResetParticle(particle);
                    }
                }
                else
                {
                    // гравитация воздействует на вектор скорости, поэтому пересчитываем его
                    particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;

                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;
                }
            }

            // этот новый цикл также будет срабатывать только в самом начале работы эмиттера
            // собственно пока не накопится критическая масса частиц
            while (particlesToCreate >= 1)
            {
                particlesToCreate -= 1;
                var particle = CreateParticle();
                ResetParticle(particle);
                particles.Add(particle);
            }
        }

        public void Render(Graphics g)// функция рендеринга
        {
            // утащили сюда отрисовку частиц
            foreach (var particle in particles)
            {
                particle.Draw(g, Radius, Time);
            }
            Time += Speed;

            Direction = -Convert.ToInt32(Time * 180 / Math.PI) + 90;

            g.DrawEllipse(
               new Pen(Color.White),
               X - Radius / 2,
               Y - Radius / 2,
               Radius,
               Radius
           );
        }
    }
   
}
