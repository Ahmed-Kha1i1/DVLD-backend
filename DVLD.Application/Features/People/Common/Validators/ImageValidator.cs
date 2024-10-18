using DVLD.Application.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

public class ImageValidator : AbstractValidator<IFormFile?>
{
    private readonly ImagesOptions _imagesOptions;
    public ImageValidator(IOptionsSnapshot<ImagesOptions> imagesOptions)
    {
        _imagesOptions = imagesOptions.Value;
        When(ImageFile => ImageFile is not null, () =>
        {
            RuleFor(ImageFile => ImageFile)
            .Must(ValidExtension)
                .WithMessage($"Invalid image type. Allowed types are: ${string.Join(", ", _imagesOptions.AllowedExtensions)}")
            .Must(ValidSize)
                .WithMessage($"Image size exceeds the maximum allowed size of ${_imagesOptions.MaxSizeInMegaBytes} MB.");
        });

    }

    private bool ValidExtension(IFormFile? file)
    {
        if (file == null) return false;

        var fileExtension = file.FileName.Split('.').Last().ToLower();
        return _imagesOptions.AllowedExtensions.Contains($"{fileExtension}");
    }


    private bool ValidSize(IFormFile? file)
    {
        if (file == null) return false;

        var maxSizeInBytes = _imagesOptions.MaxSizeInMegaBytes * 1024 * 1024;
        return file.Length <= maxSizeInBytes;
    }
}