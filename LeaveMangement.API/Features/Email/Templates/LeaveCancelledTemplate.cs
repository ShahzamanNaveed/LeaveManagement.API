namespace LeaveManagement.API.Features.Email.Templates
{
    public static class LeaveCancelledTemplate
    {
        public static string Build(
            string employeeName,
            string leaveType,
            DateTime startDate,
            DateTime endDate)
        {

            string content =
                $"""
                <p>
                    A leave request has been cancelled by the employee.
                </p>


                <p>
                    The pending approval process has been stopped.
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
                            Employee
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {employeeName}
                        </td>
                    </tr>


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


                </table>


                <p style="margin-top:20px;">
                    No further action is required for this request.
                </p>
                """;


            return EmailTemplateBuilder.Build(
                "Leave Request Cancelled",
                content,
                "CANCELLED",
                "#6c757d");
        }
    }
}