using System.Windows;
using System.Windows.Forms;
using WpfTopper.Code;

namespace WpfTopper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private KeyboardHook _hook;
        private bool _active = true;

        public MainWindow()
        {
            InitializeComponent();
            Hide();
            RegisterHotkeys();
        }

        void RegisterHotkeys()
        {
            _hook = new KeyboardHook();
            _hook.KeyPressed += HookKeyPressed;

            const ModifierKeys modifier = ModifierKeys.Control | ModifierKeys.Shift;
            //const ModifierKeys modifier = ModifierKeys.Control;
            _hook.RegisterHotKey(modifier, Keys.Enter);
        }

        void HookKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (_active)
            {
                Show();
                Activate();
                ShowActivated = true;
                _active = false;
            }
            else
            {
                Hide();
                ShowActivated = false;
                _active = true;
            }
        }
    }
}
