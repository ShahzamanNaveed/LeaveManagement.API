namespace LeaveManagement.API.Features.Email.Templates
{
    public static class LeaveApprovedTemplate
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
                    Your leave request has been approved successfully.
                </p>


                <p>
                    All assigned managers have approved your leave request.
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
                    Your leave balance has been updated accordingly.
                </p>
                """;


            return EmailTemplateBuilder.Build(
                "Leave Request Approved",
                content,
                "APPROVED",
                "#28a745");
        }
    }
}