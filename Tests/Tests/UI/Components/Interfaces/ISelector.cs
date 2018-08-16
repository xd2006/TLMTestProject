
namespace Tests.UI.Components.Interfaces
{
    interface ISelector
    {
        void SelectOption(string optionName);

        string SelectedOption();
    }
}
