namespace LeaveManagement.API.Features.Email.Templates
{
    public static class LeaveRejectedTemplate
    {
        public static string Build(
            string leaveType,
            DateTime startDate,
            DateTime endDate,
            double numberOfDays)
        {

            string content =
                $"""
                <p>
                    Your leave request has been rejected.
                </p>


                <p>
                    One of your assigned managers has rejected this leave request.
                </p>


                <h4>
                    Leave Details
                </h4>


                <table style="
                    width:100%;
                    border-collapse:collapse;
                    margin-top:15px;
                ">

                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            Leave Type
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {leaveType}
                        </td>
                    </tr>


                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            Start Date
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {startDate:dd-MM-yyyy}
                        </td>
                    </tr>


                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            End Date
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {endDate:dd-MM-yyyy}
                        </td>
                    </tr>


                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            Number Of Days
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {numberOfDays}
                        </td>
                    </tr>


                </table>


                <p style="margin-top:20px;">
                    Please contact your manager if you need further clarification.
                </p>
                """;


            return EmailTemplateBuilder.Build(
                "Leave Request Rejected",
                content,
                "REJECTED",
                "#dc3545");
        }
    }
}