using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace L12Boss19
{

    internal class ClassBall
    {
        public int radius;
        public Color color;
        public Point position;
        public Point velocity;
        public Size clientSize;
        public bool isCollision = false;
        Random r = new Random();
        public virtual void Draw(Graphics g)
        {
            Brush b = new SolidBrush(color);
            g.FillEllipse(b, position.X - radius, position.Y - radius, radius * 2, radius * 2);
        }

        public virtual void Move()
        {
            position.X -= velocity.X;
            position.Y -= velocity.Y;

        }
        public void Move(ClassPlane bk)
        {
            position.X += velocity.X;
            position.Y += velocity.Y;
            isCollision = isCollide(bk);
        }
        public void Move(ClassBoss bk)
        {
            position.X -= velocity.X;
            position.Y -= velocity.Y;
            isCollision = isCollide(bk);
        }
        bool isCollide(ClassBoss bar)
        {
            Rectangle ra = new Rectangle(bar.position.X, bar.position.Y, bar.bkSize.Width, bar.bkSize.Height);
            Rectangle rb = new Rectangle(position.X - radius, position.Y - radius, radius * 2, radius * 2);
            if (ra.IntersectsWith(rb))
                return true;
            else
                return false;
        }
        bool isCollide(ClassPlane bar)
        {
            Rectangle ra = new Rectangle(bar.position.X, bar.position.Y, bar.bkSize.Width, bar.bkSize.Height);
            Rectangle rb = new Rectangle(position.X - radius, position.Y - radius, radius * 2, radius * 2);
            if (ra.IntersectsWith(rb))
                return true;
            else
                return false;
        }
        public bool isDead()
        {
            Rectangle ra = new Rectangle(0, 0, clientSize.Width, clientSize.Height);
            Rectangle rb = new Rectangle(position.X - radius, position.Y - radius, radius * 2, radius * 2);
            if (ra.IntersectsWith(rb))
                return false;
            else
                return true;
        }

    }


    internal class ClassBoss : ClassBall
    {
        public int dir;
        public Size bkSize;
        public int life;
        public override void Move()
        {
            position.X += velocity.X;
            if (position.X > clientSize.Width - bkSize.Width)
            {
                velocity.X = -velocity.X;
                position.X = clientSize.Width - bkSize.Width;
            }
            else if (position.X < radius)
            {
                velocity.X = -velocity.X;
                position.X = radius;
            }
        }
        public override void Draw(Graphics g)
        {
            //base.Draw(g);
            Brush b = new SolidBrush(color);
            g.FillRectangle(b, new Rectangle(position, bkSize));
        }
    }

    internal class ClassPlane : ClassBall
    {
        public int dir;
        public Size bkSize;
        public bool isMove = false;
        public void DrawImage(Graphics g, Bitmap bmp)
        {
            //base.Draw(g);
            bmp.MakeTransparent(Color.White);
            g.DrawImage(bmp, new Rectangle(position, bkSize));
        }
        public override void Move()
        {
            if (dir == 1 && isMove)
                position.X -= velocity.X;
            if (dir == 3 && isMove)
                position.X += velocity.X;
            if (dir == 2 && isMove)
                position.Y -= velocity.Y;
            if (dir == 4 && isMove)
                position.Y += velocity.Y;
        }
    }
}
