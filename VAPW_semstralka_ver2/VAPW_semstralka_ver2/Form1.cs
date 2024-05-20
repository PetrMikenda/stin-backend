using Mikenda_mycka;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VAPW_semstralka_ver2
{
    public partial class Form1 : Form
    {
        bool modelConnection;
        Thread animation;
        System.Windows.Forms.Timer timer;
        bool animationRunning = false;
        static Dictionary<string, int> programs = new Dictionary<string, int>(){
                { "rychlý", 1000 },
                { "standart", 2500 },
                { "delux", 9000 }
            };
        Mycka mycka = new Mycka(programs);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {



            Point carsPosition = new Point(0, 270);
            Point frontDoorPostion = new Point(250, 231);
            Point rearDoorsPosition = new Point(550, 231);
            Point washingHeadPosition = new Point(300, 331);

            car.Location = carsPosition;
            car.SizeMode = PictureBoxSizeMode.StretchImage;
            car.Image = Image.FromFile("C:\\Users\\petrm\\Downloads\\1299198.png");

            frontDoor.Location = frontDoorPostion;
            rearDoor.Location = rearDoorsPosition;
            washingHead.Location = washingHeadPosition;

            programStart.Enabled = false;
            washingHead.Visible = false;

            inFrontOfIndic.BackColor = Color.Red;

            CustomDialog dialog = new CustomDialog();

            chooseProgram.DataSource = new BindingSource(mycka.programs.Keys, null);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                modelConnection = dialog.modelConnection;
                if (dialog.modelConnection)
                {
                    mycka.CarInChanged += ToWasher;
                    mycka.ProgramRunningChanged += WashingProcess;
                    mycka.CarOutChanged += new Action(() =>
                    {
                        animationRunning = false;
                    });
                }
                else
                {
                    timer = new System.Windows.Forms.Timer();
                    timer.Interval = 20;
                    timer.Tick += new EventHandler(Timer_Tick);
                    timer.Start();
                }
            }
            else
            {
                modelConnection = true; //pokud uzivatel zavre dialog nastavi se napojeni na model EVENT
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            switch (mycka.washerStatus)
            {
                case washerState.carInFrontOf:
                    if (!animationRunning)
                    {
                        toWasher.Enabled = true;
                        programStart.Enabled = false;
                    }

                    break;

                case washerState.carIn:
                    if (!animationRunning)
                    {
                        ToWasher();
                    }
                    break;
                case washerState.programRunning:
                    if (!animationRunning)
                    {
                        WashingProcess();
                    }
                    break;
                case washerState.carOut:
                    animationRunning = false;
                    break;

            }
        }
        private void WashingProcess()
        {
            animationRunning = true;
            animation = new Thread(WashingProcessAnimation);
            animation.Start();
        }
        private void toWasher_Click(object sender, EventArgs e)
        {
            toWasher.Enabled = false;
            mycka.carEntry();
        }
        private void programStart_Click(object sender, EventArgs e)
        {
            programStart.Enabled = false;
            washingIndic.BackColor = Color.Red;
            mycka.startProgram();
            animationRunning = false;
        }
        private void ToWasher()
        {
            programStart.Enabled = false;
            animationRunning = true;
            animation = new Thread(() =>
            {
                OpenFrontDoor();
                CarEntry();
                CloseFrontDoor();
                programStart.Invoke(new Action(() =>
                {
                    programStart.Enabled = true;
                }));
                inFrontOfIndic.Invoke(new Action(() =>
                {
                    inFrontOfIndic.BackColor= Color.Transparent;
                }));
                inIndic.Invoke(new Action(() =>
                {
                    inIndic.BackColor = Color.Red;
                }));
                traficLight.Invoke(new Action(() =>
                {
                    traficLight.BackColor = Color.Red;
                }));
            });
            animation.Start();

        }
        private void FromWasher()
        {
            inIndic.Invoke(new Action(() =>
            {
                inIndic.BackColor = Color.Transparent;
            }));
            animation = new Thread(() =>
            {
                OpenRearDoor();
                CarExit();
                CloseRearDoor();
                traficLight.Invoke(new Action(() =>
                {
                    traficLight.BackColor= Color.Green;
                }));
                car.Invoke(
                    new Action(() =>
                    {
                        car.Location = new Point(0, car.Location.Y);
                    }
                ));
                inFrontOfIndic.Invoke(new Action(() =>
                {
                    inFrontOfIndic.BackColor = Color.Red;
                }));

                animationRunning = false;
                if (modelConnection)
                {
                    toWasher.Invoke(new Action(() =>
                    {
                        toWasher.Enabled = true;
                    }));
                }
            });
            animation.Start();
        }

        private void OpenFrontDoor()
        {
            while (frontDoor.Location.Y > 110)
            {
                if (frontDoor.InvokeRequired)
                {
                    frontDoor.Invoke(new Action(() =>
                    {
                        frontDoor.Location = new Point(frontDoor.Location.X, frontDoor.Location.Y - 5);
                    }));
                }
                else
                {
                    frontDoor.Location = new Point(frontDoor.Location.X, frontDoor.Location.Y - 5);
                }
                Thread.Sleep(20);
            }
        }

        private void CloseFrontDoor()
        {
            while (frontDoor.Location.Y < 231)
            {
                if (frontDoor.InvokeRequired)
                {
                    frontDoor.Invoke(new Action(() =>
                    {
                        frontDoor.Location = new Point(frontDoor.Location.X, frontDoor.Location.Y + 5);
                    }));
                }
                else
                {
                    frontDoor.Location = new Point(frontDoor.Location.X, frontDoor.Location.Y + 5);
                }
                Thread.Sleep(20);
            }
        }

        private void CarEntry()
        {
            while (car.Location.X < 300)
            {
                if (car.InvokeRequired)
                {
                    car.Invoke(new Action(() =>
                    {
                        car.Location = new Point(car.Location.X + 5, car.Location.Y);
                    }));
                }
                else
                {
                    car.Location = new Point(car.Location.X + 5, car.Location.Y);
                }
                Thread.Sleep(20);
            }
        }
        private void WashingProcessAnimation()
        {
            animationRunning = true;
            washingIndic.Invoke(new Action(() =>
            {
                washingIndic.BackColor = Color.Red;
            }));
            washingHead.Invoke(new Action(() =>
            {
                washingHead.Visible = true;
            }));
            while (animationRunning)
            {
                for (int i = 0; i < 15; i++)
                {
                    MoveWaterUp();
                    Thread.Sleep(20);
                }
                for (int i = 0; i < 15; i++)
                {
                    MoveWaterDown();
                    Thread.Sleep(20);
                }
            }
            washingIndic.Invoke(new Action(() =>
            {
                washingIndic.BackColor = Color.Transparent;
            }));
            animationRunning = true;
            washingHead.Invoke(new Action(() =>
            {
                washingHead.Visible = false;
            }));
            programStart.Invoke(new Action(() =>
            {
                programStart.Enabled = false;
            }));
            FromWasher();
            mycka.carInFrontOf();
        }
        private void MoveWaterDown()
        {
            if (washingHead.InvokeRequired)
            {
                washingHead.Invoke((MethodInvoker)delegate
                {
                    washingHead.Location = new Point(washingHead.Location.X, washingHead.Location.Y + 5);
                });
            }
            else
            {
                washingHead.Location = new Point(washingHead.Location.X, washingHead.Location.Y + 5);
            }
        }

        private void MoveWaterUp()
        {
            if (washingHead.InvokeRequired)
            {
                washingHead.Invoke((MethodInvoker)delegate
                {
                    washingHead.Location = new Point(washingHead.Location.X, washingHead.Location.Y - 5);
                });
            }
            else
            {
                washingHead.Location = new Point(washingHead.Location.X, washingHead.Location.Y - 5);
            }
        }
        private void OpenRearDoor()
        {
            while (rearDoor.Location.Y > 110)
            {
                if (rearDoor.InvokeRequired)
                {
                    rearDoor.Invoke(new Action(() =>
                    {
                        rearDoor.Location = new Point(rearDoor.Location.X, rearDoor.Location.Y - 5);
                    }));
                }
                else
                {
                    rearDoor.Location = new Point(rearDoor.Location.X, rearDoor.Location.Y - 5);
                }
                Thread.Sleep(20);
            }
        }
        private void CloseRearDoor()
        {
            while (rearDoor.Location.Y < 231)
            {
                if (rearDoor.InvokeRequired)
                {
                    rearDoor.Invoke(new Action(() =>
                    {
                        rearDoor.Location = new Point(rearDoor.Location.X, rearDoor.Location.Y + 5);
                    }));
                }
                else
                {
                    rearDoor.Location = new Point(rearDoor.Location.X, rearDoor.Location.Y + 5);
                }
                Thread.Sleep(20);
            }
        }
        private void CarExit()
        {
            while (car.Location.X < 800)
            {
                if (car.InvokeRequired)
                {
                    car.Invoke(new Action(() =>
                    {
                        car.Location = new Point(car.Location.X + 5, car.Location.Y);
                    }));
                }
                else
                {
                    car.Location = new Point(car.Location.X + 5, car.Location.Y);
                }
                Thread.Sleep(20);
            }
        }

        private void chooseProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            mycka.ChooseProgram(chooseProgram.SelectedItem.ToString());


        }
    }
}
