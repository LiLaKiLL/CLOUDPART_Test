$(function () {
    function updtable() {
        $.ajax({
            type: "POST",
            url: "/get/" + $("#txtFilter").val(),
            success: function (d) {
                var $tbody = $('#myTable tbody').empty();
                if (d.length) {
                    $.each(d, function (i, r) {
                        $tbody.append(`<tr><td>${i + 1}</td><td>${r.id}</td><td>${r.name}</td><td>${r.description}</td><td><button type="button" class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#exampleModal" id="updproduct">Update</button> <button type="button" class="btn btn-danger" id="delproduct">Delete</button></td></tr>`);
                    });
                } else {
                    $tbody.append('<tr><td colspan="5">Empty table!</td><tr>');
                }
            }
        });
    }

    $(document).on('click', '#updproduct', function () {
        $(".modal-title").html("Update product");
        var $td = $(this).closest("tr").find("td");
        $("#txtid").val($td.eq(1).text());
        $("#txtname").val($td.eq(2).text());
        $("#txtdescription").val($td.eq(3).text());
        $(".confirm-btn").attr("data", "update");
    });

    $(document).on('click', '#addproduct', function () {
        $(".modal-title").html("Add product");
        $("#txtid, #txtname, #txtdescription").val('');
        $(".confirm-btn").attr("data", "create");
    });

    $(document).on('click', '#delproduct', function () {
        if (confirm("Want to delete?")) {
            var $tr = $(this).closest("tr");
            $.get("/delete/" + $tr.find("td:eq(1)").text(), function (r) {
                if (r.result_status === 204) {
                    $tr.remove();
                    alertify.success("Product deleted!");
                } else {
                    alertify.error("Problem");
                }
            });
        }
    });

    $(document).on('click', '.confirm-btn', function () {
        var data = {
            id: $("#txtid").val(),
            name: $("#txtname").val(),
            description: $("#txtdescription").val()
        };
        var action = $(this).attr("data");
        $.ajax({
            type: "POST",
            url: "/" + (action === "create" ? "create" : "update"),
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (r) {
                if (r.result_status === 201 || r.result_status === 204) {
                    updtable();
                    alertify.success(`Product ${action === "create" ? "created" : "updated"}!`);
                } else {
                    alertify.error("Problem");
                }
            }
        });
    });

    $(document).on('click', '#filterbtn', updtable);
});