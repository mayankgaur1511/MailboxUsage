function openModal() {
	$("#btnSave").attr('disabled', 'disabled');
	document.getElementById('modal').style.display = 'block';
	document.getElementById('fade').style.display = 'block';
}
function closeModal() {
	document.getElementById('modal').style.display = 'none';
	document.getElementById('fade').style.display = 'none';
	$("btnSave").removeAttr('disabled');
}

$('#ddlUsers').change(function () {
	openModal();
	$('#partialPlaceHolder').html(' ');
	/* Get the selected value of dropdownlist */
	var selectedID = $(this).val();

	$.ajax({
		type: "Get",
		url: "/Home/_DisplayUserDetails",
		data: { "UserID": selectedID },
		cache: false,
		success: function (data) {
			$('#partialPlaceHolder').html(data);
			/* little fade in effect */
			$('#partialPlaceHolder').fadeIn('fast');
		},
		fail: function (data) {
			$('#partialPlaceHolder').html(' ');
			/* little fade in effect */
			$('#partialPlaceHolder').fadeIn('fast');
		}
	});
	closeModal();
});