namespace Simple_Musical_Player.Models
{
    public class ResultModel<T>
    {
        /// <summary>
        /// String that should be used for transfer the error message if needed.
        /// </summary>
        public string Message;

        /// <summary>
        /// Object that should be used for transfer some data
        /// </summary>
        public T Content;

        /// <summary>
        /// Providing the status of answer
        /// </summary>
        public ResultStatus Status;

        /// <summary>
        /// Constructor with only status
        /// </summary>
        /// <param name="status">Status of answer</param>
        public ResultModel(ResultStatus status){
            this.Status = (ResultStatus)(int)status > 0 ? status : (ResultStatus)(-1);
        }

        /// <summary>
        /// Creates ResultModel instance with status and message
        /// </summary>
        /// <param name="status">Status of answer</param>
        /// <param name="message">Error message</param>
        public ResultModel(ResultStatus status, string message)
        {
            this.Status = (ResultStatus)(int)status > 0 ? status : (ResultStatus)(-1);
            this.Message = message;
        }

        /// <summary>
        /// Creates ResultModel instance with status, message and some content
        /// </summary>
        /// <param name="status">Status of answer</param>
        /// <param name="message">Error message</param>
        /// <param name="content">Content of answer</param>
        public ResultModel(ResultStatus status, string message, T content)
        {
            this.Status = (ResultStatus)(int)status > 0 ? status : (ResultStatus)(-1);
            this.Message = message;
            this.Content = content;
        }

        /// <summary>
        /// Creates ResultModel instance with status and some content
        /// </summary>
        /// <param name="status">Status of answer</param>
        /// <param name="content">Content of answer</param>
        public ResultModel(ResultStatus status, T content)
        {
            Content = content;
            Status = (ResultStatus)(int)status > 0 ? status : (ResultStatus)(-1);
        }
    }

    public enum ResultStatus
    {
        SUCCES = 1,
        ERROR = 0,
        UNDEFINED = -1
    }
}
