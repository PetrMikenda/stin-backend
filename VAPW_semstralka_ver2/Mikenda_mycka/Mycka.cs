namespace Mikenda_mycka
{
    public enum washerState
    {
        carInFrontOf,
        carIn,
        programRunning,
        carOut
    }
    public class Mycka
    {
        private washerState _washerStatus;
        public washerState washerStatus {
            get => _washerStatus;
            set {
                if (_washerStatus != value)
                {
                    _washerStatus = value;
                    OnStateChanged(_washerStatus);
                }
            }
        }
        public event Action CarInChanged;
        public event Action ProgramRunningChanged;
        public event Action CarOutChanged;

        private void OnStateChanged(washerState newState)
        {
            switch (newState)
            {
                case washerState.carIn:
                    CarInChanged?.Invoke();
                    break;
                case washerState.programRunning:
                    ProgramRunningChanged?.Invoke();
                    break;
                case washerState.carOut:
                    CarOutChanged?.Invoke();
                    break;
            }
        }
        public int chosenProgram {  get; protected set; }
        public Dictionary<string, int> programs { get; protected set; }

        public Mycka(Dictionary<string,int> Programs) 
        {
            programs = Programs;
            washerStatus = washerState.carInFrontOf;
        }
        public void carInFrontOf()
        {
            washerStatus = washerState.carInFrontOf;
        }
        public void carEntry() 
        {
            washerStatus = washerState.carIn;
        }
        public void carExit()
        {
            washerStatus = washerState.carOut;
        }
        public void startProgram()
        {
            washerStatus = washerState.programRunning;
            Thread washingThread = new Thread(() =>
            {
                Thread.Sleep(chosenProgram);
                carExit();
            });
            washingThread.Start();
        }
        public void ChooseProgram(string program)
        {
            chosenProgram = programs[program];
        }
    }
}
