namespace LeaveManagement.API.Features.Email.Templates
{
    public static class EmailTemplateBuilder
    {

        public static string Build(
            string title,
            string content,
            string status,
            string statusColor)
        {

            return $"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="UTF-8">
                <title>{title}</title>
            </head>

            <body style="font-family: Arial, sans-serif; background-color:#f4f6f8; padding:20px;">

                <div style="
                    max-width:600px;
                    margin:auto;
                    background:white;
                    border-radius:8px;
                    overflow:hidden;
                    box-shadow:0 2px 8px rgba(0,0,0,0.1);
                ">

                    <!-- Header -->

                    <div style="
                        background:#1f4e79;
                        color:white;
                        padding:20px;
                        text-align:center;
                    ">
                        <h2>
                            Leave Management System
                        </h2>
                    </div>


                    <!-- Body -->

                    <div style="
                        padding:30px;
                    ">

                        <h3>
                            {title}
                        </h3>


                        <div style="
                            display:inline-block;
                            padding:8px 15px;
                            border-radius:20px;
                            background:{statusColor};
                            color:white;
                            font-weight:bold;
                            margin-bottom:20px;
                        ">
                            {status}
                        </div>


                        <div>
                            {content}
                        </div>


                    </div>


                    <!-- Footer -->

                    <div style="
                        background:#f1f1f1;
                        padding:15px;
                        text-align:center;
                        font-size:12px;
                        color:#666;
                    ">

                        This is an automated email.
                        <br/>

                        Leave Management System

                    </div>


                </div>

            </body>
            </html>
            """;

        }

    }
}