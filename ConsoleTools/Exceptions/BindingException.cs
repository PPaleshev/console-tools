using System;


namespace ConsoleTools.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при ошибках связывания.
    /// </summary>
    public class BindingException : ApplicationException
    {
        /// <summary>
        /// Создаёт новый экземпляр исключения с описанием причины ошибки.
        /// </summary>
        /// <param name="message">Текст, описывающий причину ошибки.</param>
        public BindingException(string message) : base(message)
        {
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Создаёт новый экземпляр исключения.
        /// </summary>
        /// <param name="message">Текст, описывающий причину ошибки.</param>
        /// <param name="innerException">Объект возникшего исключения.</param>
        public BindingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}