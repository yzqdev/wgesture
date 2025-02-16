using System.ComponentModel;
using System.Windows.Forms;
using WGestures.Core.Commands;

namespace WGestures.App.Gui.Windows.CommandViews
{
    public partial class CommandViewUserControl : UserControl,ICommandView
    {
        public CommandViewUserControl()
        {
            InitializeComponent();
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual AbstractCommand Command { get; set; }

        public event CommandValueChangedEventHandler CommandValueChanged;

        protected virtual void OnCommandValueChanged()
        {
            if (CommandValueChanged != null) CommandValueChanged(Command);
        }

        public delegate void CommandValueChangedEventHandler(AbstractCommand command);
    }
}
