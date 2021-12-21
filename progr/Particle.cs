using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing; // чтобы исплоьзовать Graphics

namespace progr
{
    public class ParticleColorful : Particle  // новый класс для цветных частиц
    {
        // два новых поля под цвет начальный и конечный
        public Color FromColor;
        public Color ToColor;

        // для смеси цветов
        public static Color MixColor(Color color1, Color color2, float k)
        {
            return Color.FromArgb(
                (int)(color2.A * k + color1.A * (1 - k)),
                (int)(color2.R * k + color1.R * (1 - k)),
                (int)(color2.G * k + color1.G * (1 - k)),
                (int)(color2.B * k + color1.B * (1 - k))
            );
        }

        // ну и отрисовку перепишем
        public override void Draw(Graphics g, int circleRadius, float time)
        {
            float k = Math.Min(1f, Life / 100);

            // так как k уменьшается от 1 до 0, то порядок цветов обратный
            var color = MixColor(ToColor, FromColor, k);
            var b = new SolidBrush(color);
            x = X - Radius + (circleRadius / 2) * Convert.ToSingle(Math.Cos(time));
            y = Y - Radius + (circleRadius / 2) * Convert.ToSingle(Math.Sin(time));

            g.FillEllipse(b, x , y, Radius * 2, Radius * 2);

            b.Dispose();
        }
    }
    public class Particle
    {
        public int Radius; // радиус частицы
        public float X; // X координата положения частицы в пространстве
        public float Y; // Y координата положения частицы в пространстве
        public float x, y;

        public float SpeedX; // направление движения
        public float SpeedY; // скорость перемещения
        public virtual void Draw(Graphics g, int circleRaduis, float time)//  virtual чтобы переопределить функцию
        {
            // рассчитываем коэффициент прозрачности по шкале от 0 до 1.0
            float k = Math.Min(1f, Life / 100);
            // рассчитываем значение альфа канала в шкале от 0 до 255
            // по аналогии с RGB, он используется для задания прозрачности
            int alpha = (int)(k * 255);

            // создаем цвет из уже существующего, но привязываем к нему еще и значение альфа канала
            var color = Color.FromArgb(alpha, Color.Black);
            var b = new SolidBrush(color);

            // нарисовали залитый кружок радиусом Radius с центром в X, Y
            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);

            // удалили кисть из памяти, вообще сборщик мусора рано или поздно это сам сделает
            // но документация рекомендует делать это самому
            b.Dispose();
        }
       
        
        public float Life; // запас здоровья частицы

        public static Random rand = new Random();// добавили генератор случайных чисел

        public Particle()
        { // генерируем произвольное направление и скорость
            var direction = (double)rand.Next(360);
            var speed = 1 + rand.Next(10);

            // рассчитываем вектор скорости
            SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
            SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

            
            Radius = 2 + rand.Next(10);
            Life = 20 + rand.Next(100);
        }

    }  
}
