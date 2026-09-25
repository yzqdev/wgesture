using System.ComponentModel;
using WGestures.Core.Commands;

namespace WGestures.App.Gui.CommandViews;

public partial class GeneralNoParameterCommandView : CommandViewUserControl
{
    public GeneralNoParameterCommandView()
    {
        InitializeComponent();
    }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public override AbstractCommand Command { get; set; }
}