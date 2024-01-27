namespace Core.Constants;

public static class ArticleConstants
{
    public const string TitleRequired = "Title can't be empty";
    public const string DescriptionRequired = "Description can't be empty";
    public const string BodyRequired = "Body can't be empty";
    public const string AuthorRequired = "Author can't be empty";
    
    public const string TitleLengthExceeded = "Title is too long";
    public const string DescriptionLengthExceeded = "Description is too long";
    public const string BodyLengthExceeded = "Body is too long";
    
    public const string AuthorNotFound = "Author not found";
    
    public const int TitleMaxLength = 100;
    public const int SlugMaxLength = 72;
    public const int DescriptionMaxLength = 200;
    public const int BodyMaxLength = 10000;
}
