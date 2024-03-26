using apiclient.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace apiclient.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly NetUtils _netUtils;

    [ObservableProperty]
    private bool _isInternetErrorVisible;

    public RelayCommand UploadButtonClickedCommand { get; }

    public MainPageViewModel(NetUtils netUtils)
    {
        _netUtils = netUtils;

        UploadButtonClickedCommand = new RelayCommand(async () => await UploadButtonClicked());
    }

    private async Task UploadButtonClicked()
    {
        var results = await FilePicker.PickMultipleAsync(new PickOptions
        {
            PickerTitle = "Выбирите изображения",
            FileTypes = FilePickerFileType.Images
        });

        foreach (var result in results)
        {
            var fileStream = await result.OpenReadAsync();
            var details = await _netUtils.SendImageAsync(fileStream, result.FileName);
        }
    }
}