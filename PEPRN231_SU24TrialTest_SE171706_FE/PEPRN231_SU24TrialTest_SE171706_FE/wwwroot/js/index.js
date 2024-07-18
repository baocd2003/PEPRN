const apiUrl = "http://localhost:5264/api/paintings"
$(document).ready(function () {
    let token = sessionStorage.getItem("authToken");
    console.log(token);
    if (token == null && token == undefined) {
        window.location.href = '/Index';
    }
    const loadPaintings = () => {
        $.ajax({
            url: apiUrl,
            type: 'GET',
            success: (data) => {
                renderPaintings(data);
                console.log(data);
            }
        })
    }

    loadPaintings();

    
    
    const renderPaintings = (paintings) => {
        let rows = '';
        paintings.forEach(painting => {
            rows += `
                <tr>
                <td>${painting.paintingId}</td>
                <td>${painting.paintingName}</td>
                <td>${painting.paintingDescription}</td>
<td>${painting.paintingAuthor}</td>
<td>${painting.price}</td>
<td>${painting.publishYear}</td>
<td>${painting.createdDate}</td>
<td>${painting.styleName}</td>
                <td>
                   
                    <button class="btn btn-danger btn-sm deletePainting" id="delBtn" type="submit" data-id="${painting.paintingId}">Delete</button>
                    <button class="btn btn-warning btn-sm updatePainting" id="updateBtn" type="submit" data-id="${painting.paintingId}">Update</button>
                
    </div>
</div>
                </td>

            </tr>
            `
        })
        $('#paintingsTable tbody').html(rows);
    }

    //delete paint
    $(document).on('click', '#delBtn', function (event) {
        if (token) {
            var id = $(this).data('id');
            console.log(id);
            $.ajax({
                url: `${apiUrl}/${id}`,
                type: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                success: (data) => {
                    console.log(data)
                    loadPaintings();
                },
                error: (xhr, status, error) => {
                    console.error('Error loading paintings:', error);
                }
            })
            console.log(id)
        } else {
            alert("Login please");
        }
       
    })
    const renderPaint = (paint) => {
        $('#paintingId').val(paint.paintingId);
        $('#paintingName').val(paint.paintingName);
        $('#paintingDescription').val(paint.paintingDescription);
        $('#paintingAuthor').val(paint.paintingAuthor);
        $('#price').val(paint.price);
        $('#publishYear').val(paint.publishYear);
        $('#styleId').val(paint.styleId);
    };

    //update painting
    $(document).on('click', '#updateBtn', function (event) {
            var id = $(this).data('id');
            console.log(id);
        $.ajax({
            url: `${apiUrl}/${id}`,
            type: 'GET',
            success: (data) => {
                renderPaint(data);
            }
        })
    })

    //create paint
    $('#createBtn').on('click', function (event) { 
        console.log("a")
        if (token) {     
                const paintingData = {
                    paintingName: $('#paintingName').val(),
                    paintingDescription: $('#paintingDescription').val(),
                    paintingAuthor: $('#paintingAuthor').val(),
                    price: parseFloat($('#price').val()),
                    publishYear: parseInt($('#publishYear').val()),
                    styleId: $('#styleId').val()
                };

            console.log(paintingData)
                $.ajax({
                    url: apiUrl,
                    type: 'POST',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    },
                    data: JSON.stringify(paintingData),
                    success: (data) => {
                        console.log('Painting created:', data);

                        loadPaintings();
                    },
                    error: (xhr, status, error) => {
                        console.error('Error creating painting:', error);
                        alert('An error occurred while creating the painting');
                    }
                });
        }
       
    });

    $(document).on('click', '#confirmUpdateBtn', function (event) {
        event.preventDefault();
        if (paintingId) {
            let paintingId = $('#paintingId').val()
            const paintingData = {
                paintingId: paintingId,
                paintingName: $('#paintingName').val(),
                paintingDescription: $('#paintingDescription').val(),
                paintingAuthor: $('#paintingAuthor').val(),
                price: parseFloat($('#price').val()),
                publishYear: parseInt($('#publishYear').val()),
                styleId: $('#styleId').val()
            };
            $.ajax({
                url: apiUrl,
                type: 'PUT',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                data: JSON.stringify(paintingData),
                success: (data) => {
                    console.log('Painting created:', data);

                    loadPaintings();
                },
                error: (xhr, status, error) => {
                    console.error('Error creating painting:', error);
                    alert('An error occurred while creating the painting');
                }
            });
        }
    })


    function updatePaint(event) {
        event.preventDefault();
        let paintingId = $('#paintingId').val();
        const paintingData = {
            paintingId: paintingId,
            paintingName: $('#paintingName').val(),
            paintingDescription: $('#paintingDescription').val(),
            paintingAuthor: $('#paintingAuthor').val(),
            price: parseFloat($('#price').val()),
            publishYear: parseInt($('#publishYear').val()),
            styleId: $('#styleId').val()
        };

        $.ajax({
            url: `${apiUrl}/${paintingId}`,
            type: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(paintingData),
            success: (data) => {
                console.log('Painting updated:', data);
                loadPaintings();
                resetForm();
            },
            error: (xhr, status, error) => {
                console.error('Error updating painting:', error);
                alert('An error occurred while updating the painting');
            }
        });
    }


});