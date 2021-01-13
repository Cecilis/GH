using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace GH.Utilities
{
    public class ExceptionsHandlers
    {

        public static HttpResponseMessage getHttpResponseMessage(Exception ex) {
            //DbUpdateException

           
            string exceptionMessage = string.Empty;

            if (ex.InnerException != null) {

                var internalException = ex.InnerException.InnerException != null ? ex.InnerException.InnerException : ex.InnerException;  //as SqlException;
            
                if (internalException is SqlException)
                {
                    var tm = internalException.Message;

                    switch (((SqlException)internalException).Number)
                    {   //FK exception
                        case 547:
                            //Instrucción DELETE en conflicto con la restricción REFERENCE "FK_dbo.Servicios_dbo.Clientes_Clientes_id_cliente". El conflicto ha aparecido en la base de datos "GH", tabla "dbo.Servicios", column 'Clientes_id_cliente'.
                            if (tm.ToUpper().IndexOf("DELETE") > 0)
                            {
                                exceptionMessage = "No se puede eliminar, tiene elementos relacionados";
                            }
                            if (tm.ToUpper().IndexOf("INSERT") > 0)
                            {
                                exceptionMessage = "No se puede incluir, ya existe un elemento similar a este.";
                            }
                            if (tm.ToUpper().IndexOf("UPDATE") > 0)
                            {
                                exceptionMessage = "No se puede modificar, ya existe un elemento similar a este. " + tm;
                            }
                            break;
                        //case 2627:
                        case 2601:  //No se puede insertar una fila de clave duplicada en el objeto 'dbo.Bases' con índice único 'IX_Unique_BaseNombre'. El valor de la clave duplicada es (abc).\r\nSe terminó la instrucción.                        
                            tm = tm.Substring(tm.IndexOf('(') + 1, tm.IndexOf(')') - tm.IndexOf('(') - 1);
                            exceptionMessage = "Ya existe un registro para " + tm;
                            break;
                        default:
                            exceptionMessage = internalException.Message;
                            break;
                    }
                }
                else
                {
                        //exceptionMessage = internalException.InnerException.InnerException.Message;
                    
                    if (internalException.Message != null)
                    {
                        exceptionMessage = internalException.Message;
                    }
                };
            }
            else
            {
                exceptionMessage = ex.Message;
            }

            var responseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(exceptionMessage, System.Text.Encoding.UTF8),
                StatusCode = HttpStatusCode.Conflict
            };

            return responseMessage;
        }
    }
        
}


//var response = new HttpResponseMessage(HttpStatusCode.NotFound)
//{
//    Content = new StringContent(dbException.Message, System.Text.Encoding.UTF8),
//    StatusCode = HttpStatusCode.NotFound
//};

//return BadRequest( HttpStatusCode.BadRequest,new StringContent(dbException.Message, System.Text.Encoding.UTF8));

//HttpError httpError = new HttpError(dbException.Message);
//return Content(HttpStatusCode.BadRequest, httpError);