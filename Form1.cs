namespace L12Boss19
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 1.飛機可以四個方向飛行
        /// 2.更改子彈的顏色
        /// 3.飛機子彈打到BOSS可減少boss血量
        /// 4.碰到反彈板，會朝相反方向產生球 10
        /// </summary>

        Size size;
        int ballR;
        Point pos;
        Point velocity;
        Color color;
        System.Windows.Forms.Timer timer;
        Random r = new Random();
        List<ClassBall> listBall = new List<ClassBall>();
        int w = 1024, h = 768;
        Bitmap[] bmpRole = { new Bitmap(@"res\raiden.png"), new Bitmap(@"res\raiden_s.png") };

        ClassBoss boss = new ClassBoss();
        int bosslife = 10;
        ClassPlane role = new ClassPlane();
        int roleCnt = 0;
        List<ClassBall> listBullet = new List<ClassBall>();

        int playerHP = 20;

        
        System.Media.SoundPlayer bgm = new System.Media.SoundPlayer(@"res\bgm.wav");
        private void Form1_Load(object sender, EventArgs e)
        {
            size = new Size(w, h);
            this.ClientSize = size;
            ballR = 5;
            pos = new Point(w / 2, h - 100);
            velocity = new Point(10, 10);
            color = Color.Black;
            initialBall();
            timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 16;//60 FPS
            timer.Enabled = true;
            this.DoubleBuffered = true;
            this.MouseClick += Form1_MouseClick;
            initialBar();
            
            bgm.PlayLooping();
            this.Text = "東方低配版 ～ the kockoff ver.";
        }
        private void initialBall()
        {
            ClassBall tmp = new ClassBall();
            role.clientSize = size;
            role.bkSize = new Size(40, 40);
            role.position = pos;
            role.velocity = velocity;
            role.color = Color.Orange;
        }


        //定義結構
        //struct 結構名稱
        // {
        //      資料型態1 欄位名稱1;
        //      資料型態2 欄位名稱2;
        //      資料型態3 欄位名稱3;
        //  }
        //宣告結構變數
        // struct 結構名稱 結構變數;
        //設定結構變數初值
        //結構名稱.欄位名稱 = 設定值;
        struct strBk
        {
            public Point pos;
            public Size size;
            public Color color;
        }

        private void initialBar()
        {
            //宣告a為具有strBk結構的結構陣列，大小為4
            int bw = 200;
            int bh = 20;
            strBk[] a = new strBk[4];
            //dir=0 下 
            a[0].pos = new Point(w / 2, h - bh);
            a[0].size = new Size(bw, bh);
            a[0].color = Color.Red;
            #region dir=1 左 
            a[1].pos = new Point(0, h / 2);
            a[1].size = new Size(bh, bw);
            a[1].color = Color.Green;
            #endregion
            #region  dir=2 
            a[2].pos = new Point(w / 2, 0);
            a[2].size = new Size(bw, bh);
            a[2].color = Color.Blue;
            #endregion
            #region dir=3 右
            a[3].pos = new Point(w - bh, h / 2);
            a[3].size = new Size(bh, bw);
            a[3].color = Color.Black;
            #endregion

            boss.clientSize = size;
            boss.position = a[2].pos;
            boss.bkSize = a[2].size;
            boss.color = a[2].color;
            boss.velocity = new Point(2, 0);
            boss.life = bw;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            pos = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                initialBullet();
            }
        }

        private void initialBullet()
        {
            ClassBall tmp = new ClassBall();
            tmp.clientSize = size;
            tmp.radius = 4;
            tmp.position = role.position;
            tmp.velocity = new Point(0, 10);
            tmp.color = Color.Aqua;
            listBullet.Add(tmp);
        }

        //多載:同一方法可以有符合需求的不同參數版本，這叫做方法的多載 (overloading) 。
        //傳值
        private void initialBall(int v1, int v2)
        {
            ClassBall tmp = new ClassBall();
            tmp.clientSize = size;
            tmp.radius = ballR;
            tmp.position = boss.position;
            tmp.velocity = new Point(v1, v2);
            tmp.color = Color.Orange;
            listBall.Add(tmp);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            role.Move();
            boss.Move();
            roleCnt = 0;
            boss.color = Color.Blue;
            if (r.Next() % 4 == 0)
                initialBall(0, r.Next(1, 4));
            for (int i = 0; i < listBall.Count; i++)
            {
                if (listBall[i].isDead())
                    listBall.Remove(listBall[i]);
                else
                {
                    listBall[i].Move(role);
                    if (listBall[i].isCollision)
                    {
                        playerHP -= 1;
                        listBall.Remove(listBall[i]);
                        roleCnt = 1;
                        break;
                    }
                }
            }


            #region 打BOSS
            for (int i = 0; i < listBullet.Count; i++)
            {
                if (listBullet[i].isDead())
                    listBullet.Remove(listBullet[i]);
                else
                {
                    listBullet[i].Move(boss);
                    if (listBullet[i].isCollision)
                    {
                        listBullet.Remove(listBullet[i]);
                        bosslife -= 1;

                        if ((bosslife % 10 == 0) && (bosslife >= 100))
                        {
                            boss.bkSize.Width = bosslife;
                        }


                        break;
                    }
                }
            }
            #endregion
            
            this.Invalidate();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            role.isMove = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.A:
                case Keys.Left:
                    role.dir = 1;
                    role.isMove = true;
                    break;
                case Keys.D:
                case Keys.Right:
                    role.dir = 3;
                    role.isMove = true;
                    break;
                case Keys.W:
                case Keys.Up:
                    role.dir = 2;
                    role.isMove = true;
                    break;
                case Keys.S:
                case Keys.Down:
                    role.dir = 4;
                    role.isMove = true;
                    break;
            }
        }

        

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //背景
            e.Graphics.DrawImage(new Bitmap(@"res\bg.jpg"), 0, 0);
            //boss子彈
            for (int i = 0; i < listBall.Count; i++)
                listBall[i].Draw(e.Graphics);
            //飛機子彈
            for (int i = 0; i < listBullet.Count; i++)
                listBullet[i].Draw(e.Graphics);
            //BOSS
            boss.Draw(e.Graphics);
            role.DrawImage(e.Graphics, bmpRole[roleCnt]);
            //文字
            Font font = new Font("微軟正黑體", 30);
            Brush b = new SolidBrush(Color.Aqua);
            e.Graphics.DrawString(bosslife.ToString(), font, b, new PointF(900, 20));
            e.Graphics.DrawString(playerHP.ToString(), font, b, new PointF(20, 20));

            if (playerHP <= 0)
            {
                font = new Font("微軟正黑體", 100);
                b = new SolidBrush(Color.Red);
                e.Graphics.DrawString("You Lose", font, b, new PointF(0, h / 2));
                timer.Enabled = false;
                bgm.Stop();
            }

            if (bosslife <= 0)
            {
                font = new Font("微軟正黑體", 100);
                b = new SolidBrush(Color.Gold);
                e.Graphics.DrawString("You Win", font, b, new PointF(0, h / 2));
                timer.Enabled = false;
                bgm.Stop();
                System.Media.SoundPlayer sp = new System.Media.SoundPlayer(@"res\win.wav");
                sp.Play();
            }
        }

    }
}