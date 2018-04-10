function openModal() {
	
	$("#btnSave").attr('disabled', 'disabled');
	document.getElementById('modal').style.display = 'block';
	document.getElementById('fade').style.display = 'block';
}
function closeModal() {
	document.getElementById('modal').style.display = 'none';
	document.getElementById('fade').style.display = 'none';
	$("#btnSave").removeAttr('disabled');
}


$("body").on("click", "#btnSave", function () {
	
	//Loop through the Table rows and build a JSON array.
	var usages = new Array();
	var count = 0;
	var isAnswered = true;
	$("#tblUsage TBODY TR").each(function () {
		var row = $(this);
		var usage = {};
		usage.MboxID = $("#MboxID" + count).val();
		usage.IUsed = $("#IUsed" + count).is(":checked");
		usage.IDontUse = $("#IDontUse" + count).is(":checked");
		usage.IDontKnow = $("#IDontKnow" + count).is(":checked");
		if (usage.IUsed || usage.IDontKnow  || usage.IDontUse) {
			isAnswered = true;
			usages.push(usage);
		}
		else {
			isAnswered = false;
			closeModal();
			alert("Please select appropriate radio button for all mailboxes!");
			$("#IUsed" + count).focus();
			return false;
		}
		count++;
	});
	openModal();
	if (isAnswered == true) {
		//Send the JSON array to Controller using AJAX.
		$.ajax({
			type: "POST",
			url: "/Home/Savedata",
			data: JSON.stringify(usages),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				closeModal();
				window.location.href = data.redirectTo;
			},
			fail: function (data) {
				closeModal();
				window.location.href = data.redirectTo;
			}
		});
	}
	closeModal();
});