namespace Chirp.Core

{
    using FluentValidation;
    /// <summary>
    /// This is a data transfer object which is used from the outer part of the code to be send back to the repositories when we create a Cheep
    /// We are using fluentvalidation to stop the creation of cheeps with the wrong length
    /// </summary>
    public class createCheepDTO
    {
        public AuthorDTO Author { get; set; }
        public string Message { get; set; }
        public createCheepDTO(AuthorDTO AuthorDTO, string Message)
        {
            this.Author = AuthorDTO;
            this.Message = Message;
        }
    }

    public class createCheepDTOValidator : AbstractValidator<createCheepDTO> // To use validator: https://docs.fluentvalidation.net/en/latest/start.html
    {
        public createCheepDTOValidator()
        {
            RuleFor(createCheepDTO => createCheepDTO.Message).MinimumLength(1);
            RuleFor(createCheepDTO => createCheepDTO.Message).MaximumLength(160);
        }
    }
}