using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yilanOyunu
{
    public partial class yilanOyunu : MetroForm
    {

        int[,] oyunAlani;
        int skor=0;
        int hak=3;
        bool gameOver = false;

        int direction = 6;

        int xLokasyonu = 21;
        int yLokasyonu = 15;
        int xGenisleme = 7;
        int yGenisleme = 7;

        LinkedList<int> yilanXYon=new LinkedList<int>();
        LinkedList<int> yilanYYon=new LinkedList<int>();

        Random rastgele=new Random();

        Rectangle kare =new Rectangle();
        SolidBrush yilanRenk=new SolidBrush(Color.Blue);
        SolidBrush yemRenk =new SolidBrush(Color.Red);
        Brush oyunAlaniRenk=new SolidBrush(Color.White);




        public yilanOyunu()
        {
            InitializeComponent();
            Application.AddMessageFilter(new TestMessageFilter(this));
            
            oyunAlani=new int[50,50];

            for(int i=0 ; i<50 ; i++)
            {
                for(int j=0 ; j<50 ; j++)
                {
                    oyunAlani[i,j]=0;
                }
            }

            lbl_Skor.Text="Skor : 0";
            rb_Hiz2.Checked=true;
        }


        public void oyunSifirlama()
        {
            gameOver=false;
            timer1.Enabled=false;
            skor=0;
            lbl_Skor.Text="SKOR " + skor.ToString();
            direction=6;
            xLokasyonu=21;
            yLokasyonu=15;
            xGenisleme=7;
            yGenisleme=7;

            for(int i=0 ; i<50 ; i++)
            {
                for(int j=0 ; j<50 ; j++)
                {
                    oyunAlani[i,j]=0;
                }
            }

            yilanXYon.Clear();
            yilanYYon.Clear();

            btn_BaslatDurdur.Text="OYNA";
            panel1.Invalidate();
        }

        public bool tusKontrol(Keys keyCode)
        {
            bool ret=true;

            switch(keyCode)
            {
                case Keys.A:
                    if(direction!=6)
                    {
                        direction=4;
                    }
                    break;
                case Keys.D:
                    if(direction!=4)
                    {
                        direction=6;
                    }
                    break;
                case Keys.W:
                    if(direction!=2)
                    {
                        direction=8;
                    }
                    break;
                case Keys.S:
                    if(direction!=8)
                    {
                        direction=2;
                    }
                    break;

                default:ret=false;
                    break;
            }
            return ret;
        }

        private void btn_BaslatDurdur_Click(object sender, EventArgs e)
        {
            if(timer1.Enabled==false && !gameOver)
            {
                timer1.Enabled=true;
                btn_BaslatDurdur.Text="DURDUR";
            }
            else
            {
                timer1.Enabled=false;
                btn_BaslatDurdur.Text="BAŞLAT";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(direction==8)
            {
                if(yLokasyonu==0)
                {
                    yLokasyonu=49;
                }
                else
                {
                    yLokasyonu--;
                }
            }


            else if(direction==2)
            {
                if(yLokasyonu==49)
                {
                    yLokasyonu=0;
                }
                else
                {
                    yLokasyonu++;
                }
            }


            else if(direction==4)
            {
                if(xLokasyonu==0)
                {
                    xLokasyonu=49;
                }
                else
                {
                    xLokasyonu--;
                }
            }


            else if(direction==6)
            {
                if(xLokasyonu==49)
                {
                    xLokasyonu=0;
                }
                else
                {
                    xLokasyonu++;
                }
            }



            if(oyunAlani[xLokasyonu,yLokasyonu]==1)
            {
                timer1.Enabled=false;
                gameOver=true;
                lbl_Skor.Text= "SKOR: " + skor.ToString() + " Başarısız!!!";
            }


            if(yilanXYon.Count>10 && oyunAlani[xLokasyonu,yLokasyonu]!=3)
            {
                yilanXYon.RemoveLast();
                yilanYYon.RemoveLast();
            }
            else if(oyunAlani[xLokasyonu,yLokasyonu]==3)
            {
                xGenisleme=rastgele.Next(50);
                yGenisleme=rastgele.Next(50);

                while(oyunAlani[xGenisleme,yGenisleme]==1)
                {
                    xGenisleme=rastgele.Next(50);
                    yGenisleme=rastgele.Next(50);
                }

                skor += 5* hak;
                lbl_Skor.Text= "SKOR: " + skor.ToString();
            }

            yilanXYon.AddFirst(xLokasyonu);
            yilanYYon.AddFirst(yLokasyonu);

            for(int i=0 ; i<50 ; i++)
            {
                for(int j=0 ; j<50 ; j++)
                {
                    oyunAlani[i,j]=0;
                }
            }

            LinkedListNode<int> yilanXYonitem = yilanXYon.First;
            LinkedListNode<int> yilanYYonitem =yilanYYon.First;

            while(yilanXYonitem!=yilanXYon.Last && yilanYYonitem!=yilanYYon.Last)
            {
                if(yilanXYonitem==yilanXYon.First)
                {
                    oyunAlani[yilanXYonitem.Value,yilanYYonitem.Value]=2;
                }
                else
                {
                    oyunAlani[yilanXYonitem.Value,yilanYYonitem.Value]=1;
                }

                yilanXYonitem=yilanXYonitem.Next;
                yilanYYonitem=yilanYYonitem.Next;
            }
            oyunAlani[xGenisleme,yGenisleme]=3;

            #region panel, Yılan Ve Yem Boyama
            using (Graphics gr=panel1.CreateGraphics())
            {
                using(Image im=new Bitmap(panel1.ClientRectangle.Width, panel1.ClientRectangle.Height))
                {
                    using(Graphics g=Graphics.FromImage(im))
                    {
                        kare.X=0;
                        kare.Y=0;
                        kare.Width=500;
                        kare.Height=500;
                        g.FillRectangle(oyunAlaniRenk,kare);

                        kare.Width=10;
                        kare.Height=10;


                        for(int i=0 ; i<50 ; i++)
                        {
                            for(int j=0 ; j<50 ; j++)
                            {
                                if(oyunAlani[i,j]==1)
                                {
                                    kare.X=10*i;
                                    kare.Y=10*j;
                                    g.FillRectangle(yilanRenk,kare);
                                }
                                else if(oyunAlani[i,j]==2)
                                {
                                    kare.X=10*i-2;
                                    kare.Y=10*j-2;
                                    kare.Width=14;
                                    kare.Height=14;
                                    g.FillRectangle(yilanRenk,kare);
                                    kare.Width=10;
                                    kare.Height=10;
                                }
                                else if(oyunAlani[i,j]==3)
                                {
                                    kare.X=10*i;
                                    kare.Y=10*j;
                                    g.FillRectangle(yemRenk,kare);
                                }
                            }
                        }  
                    }
                    gr.DrawImage(im,panel1.ClientRectangle);
                }
            }
            #endregion
        }

        #region radioButton Hız Kontroller
        private void rb_Hiz1_CheckedChanged(object sender, EventArgs e)
        {
            hak=1;
            timer1.Interval=150;
        }

        private void rb_Hiz2_CheckedChanged(object sender, EventArgs e)
        {
            hak=2;
            timer1.Interval=100;
        }

        private void rb_Hiz3_CheckedChanged(object sender, EventArgs e)
        {
            hak=4;
            timer1.Interval=50;
        }

        private void rb_Hiz4_CheckedChanged(object sender, EventArgs e)
        {
            hak=8;
            timer1.Interval=25;
        }

        #endregion
        public class TestMessageFilter: IMessageFilter
        {
            private yilanOyunu FOwner;

            const int WM_KEYDOWN=0X100;
            const int WM_KEYUP=0X101;
            const int WM_LEFTMOUSEDOWN = 0X201;
            const int WM_LEFTMOUSEUP = 0X202;
            const int WM_LEFTMOUSEDBL = 0X203;

            public TestMessageFilter(yilanOyunu aOwner)
            {
                FOwner=aOwner;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if(m.Msg==WM_KEYDOWN)
                    return FOwner.tusKontrol((Keys)(int)m.WParam & Keys.KeyCode);
                else
                    return false;
            }
        }

        private void btn_Sifirla_Click(object sender, EventArgs e)
        {
            oyunSifirlama();
        }


    }
}
