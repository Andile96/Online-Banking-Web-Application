$(document).ready(function () {
  
    $('#staffStudentNumberContainer').hide();
    $('#staffStudentNumberContainer label').hide();

   
    $('#userTypeDropdown').change(function () {
        var selectedUserType = $(this).val();

        
        if (selectedUserType === 'Staff' || selectedUserType === 'Student') {
            $('#staffStudentNumberContainer').show();
            $('#staffStudentNumberContainer label').show();  
        } else {
            $('#staffStudentNumberContainer').hide();
            $('#staffStudentNumberContainer label').hide();  
        }
    });
});


