class PointApp {
    constructor() {
        this.stage = null;
        this.layer = null;
        this.selectedPoint = null;
        this.selectedComment = null;
        this.isEditing = false;
    }

    init() {
        const width = $('#konva-container').width();
        const height = $('#konva-container').height();

        this.stage = new Konva.Stage({
            container: 'konva-container',
            width: width,
            height: height
        });

        this.layer = new Konva.Layer();
        this.stage.add(this.layer);

        this.bindEvents();
        this.loadPoints();

        document.getElementById('konva-container').addEventListener('contextmenu', e => e.preventDefault());
    }

    bindEvents() {
        $('#add-point-btn').on('click', () => this.showForm());
        $('#create-point-btn').on('click', () => this.createPointFromForm());
        $('#cancel-point-btn').on('click', () => this.hideForm());

        $('#add-comment-btn').on('click', () => this.addComment());
        $('#edit-point-btn').on('click', () => this.showEditForm());

        $('#save-comment-btn').on('click', () => this.saveCommentFromForm());
        $('#cancel-comment-btn').on('click', () => this.hideCommentForm());
        $('#delete-comment-btn').on('click', () => this.deleteComment());

        $(document).on('click', () => $('#context-menu').hide());
    }

    showForm(x = null, y = null) {
        this.isEditing = false;
        this.selectedPoint = null;


        $('#point-x').val(x ?? this.stage.width() / 2);
        $('#point-y').val(y ?? this.stage.height() / 2);
        $('#point-color').val('#ff0000');
        $('#point-radius').val(10);
        $('#point-form-modal').fadeIn();
    }

    hideForm() {
        $('#point-form-modal').fadeOut();
    }

    showCommentForm(comment = null) {
        this.selectedComment = comment;

        if (comment) {
            $('#comment-text').val(comment.text);
            $('#comment-bg-color').val(comment.backgroundColor || '#ffffcc');
            $('#delete-comment-btn').show();
        } else {
            $('#comment-text').val('');
            $('#comment-bg-color').val('#ffffcc');
            $('#delete-comment-btn').hide();
        }

        $('#comment-form-modal').fadeIn();
    }

    hideCommentForm() {
        $('#comment-form-modal').fadeOut();
    }

    saveCommentFromForm() {
        const text = $('#comment-text').val();
        const backgroundColor = $('#comment-bg-color').val();

        if (!text || !this.selectedPoint) return;

        if (this.selectedComment) {
            $.ajax({
                url: `/api/comments/${this.selectedComment.id}`,
                method: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify({ text, backgroundColor }),
                success: () => {
                    this.selectedPoint = null;
                    this.selectedComment = null;
                    this.layer.destroyChildren();
                    this.loadPoints();
                    this.hideCommentForm();

                },
                error: () => alert('Ошибка при редактировании комментария')
            });
        }

         else {
            $.ajax({
                url: `/api/comments`,
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    text,
                    pointId: this.selectedPoint.id,
                    backgroundColor
                }),
                success: () => {
                    this.layer.destroyChildren();
                    this.loadPoints();
                    this.hideCommentForm();
                },
                error: () => alert('Ошибка при добавлении комментария')
            });
        }
    }

    deleteComment() {
        if (!this.selectedComment) return;

        if (confirm('Удалить комментарий?')) {
            $.ajax({
                url: `/api/comments/${this.selectedComment.id}`,
                method: 'DELETE',
                success: () => {
                    this.selectedPoint = null;
                    this.selectedComment = null;
                    this.layer.destroyChildren();
                    this.loadPoints();
                    this.hideCommentForm();
                },
                error: () => alert('Ошибка при удалении комментария')
            });
        }
    }

    createPointFromForm() {
        const updated = {
            x: parseFloat($('#point-x').val()),
            y: parseFloat($('#point-y').val()),
            color: $('#point-color').val(),
            radius: parseFloat($('#point-radius').val())
        };

        if (this.isEditing && this.selectedPoint) {
            updated.id = this.selectedPoint.id;
            console.log('Creating point with:', updated);
            $.ajax({
                url: `/api/points/${updated.id}`,
                method: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(updated),
                success: () => {
                    this.selectedPoint = null;
                    this.selectedComment = null;
                    this.layer.destroyChildren();
                    this.loadPoints();
                    this.hideForm();
                },
                error: () => alert('Ошибка при обновлении точки')
            });
        } else {
            console.log('Creating point with:', updated);
            $.ajax({
                url: '/api/points',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(updated),
                success: (savedPoint) => {
                    console.log('Saved point (from server):', savedPoint);
                    const group = this.createCircle(savedPoint);
                    this.layer.add(group);
                    this.layer.draw();
                    this.hideForm();
                },
                error: () => alert('Ошибка при создании точки')
            });
        }
    }

    createCircle(point) {
        const group = new Konva.Group({
            x: point.x,
            y: point.y,
            draggable: true
        });

        const circle = new Konva.Circle({
            x: 0,
            y: 0,
            radius: point.radius,
            fill: point.color,
            stroke: 'black',
            strokeWidth: 1,
            id: point.id
        });

        group.add(circle);

        $.get(`/api/points/${point.id}/comments`, (comments) => {
            comments.sort((a, b) => a.id - b.id);

            comments.forEach((c, i) => {
                const commentGroup = new Konva.Group();

                const bg = new Konva.Rect({
                    x: 0,
                    y: circle.radius() + 5 + i * 22,
                    height: 20,
                    fill: c.backgroundColor || '#ffffcc',
                    cornerRadius: 4
                });

                const text = new Konva.Text({
                    x: circle.x(),
                    y: circle.y() + circle.radius() + 5 + i * 22,
                    text: `comment${c.id}`,
                    fontSize: 14,
                    fill: 'black',
                    name: `comment-${point.id}`,
                    id: `comment-${c.id}`,
                    padding: 4
                });

                text.offsetX(text.width() / 2);
                bg.offsetX(text.width() / 2);
                bg.width(text.width() + 8);

                text.on('click', () => {
                    const isExpanded = text.text().includes(c.text);
                    text.text(isExpanded ? `comment${c.id}` : `comment${c.id} ${c.text} comment${c.id}`);
                    text.offsetX(text.width() / 2);
                    bg.offsetX(text.width() / 2);
                    bg.width(text.width() + 8);
                    this.layer.draw();
                });

                text.on('contextmenu', (e) => {
                    e.evt.preventDefault();
                    this.selectedPoint = point;
                    this.showCommentForm(c);
                });

                commentGroup.add(bg);
                commentGroup.add(text);
                group.add(commentGroup);
            });
            this.layer.draw();
        });

        circle.on('mousedown', (e) => {
            if (e.evt.button !== 2) return;

            e.evt.preventDefault();
            e.evt.stopPropagation();
            this.selectedPoint = point;
            const mousePos = this.stage.getPointerPosition();

            $('#context-menu')
                .css({ top: mousePos.y + 'px', left: mousePos.x + 'px' })
                .show();
        });

        circle.on('dblclick', () => {
            if (confirm('Удалить точку?')) {
                $.ajax({
                    url: `/api/points/${point.id}`,
                    type: 'DELETE',
                    success: () => {
                        group.destroy();
                        this.layer.draw();

                        if (this.selectedPoint && this.selectedPoint.id === point.id) {
                            this.selectedPoint = null;
                            this.selectedComment = null;
                        }

                        $('#context-menu').hide();
                        $('#comment-form-modal').hide();
                    },
                    error: () => alert('Ошибка при удалении точки')
                });
            }
        });

        group.on('dragmove', () => {
            const circle = group.findOne('Circle');
            const comments = group.find('Text');
            comments.forEach((text, i) => {
                text.position({
                    x: circle.x(),
                    y: circle.y() + circle.radius() + 5 + i * 18
                });
                text.offsetX(text.width() / 2);
            });
        });

        group.on('dragend', () => {
            const absPos = group.getAbsolutePosition();

            point.x = absPos.x;
            point.y = absPos.y;

            if (this.selectedPoint && this.selectedPoint.id === point.id) {
                this.selectedPoint.x = point.x;
                this.selectedPoint.y = point.y;
            }

            $.ajax({
                url: `/api/points/${point.id}`,
                method: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify({
                    id: point.id,
                    x: point.x,
                    y: point.y,
                    color: point.color,
                    radius: point.radius
                }),
                error: () => alert('Ошибка при сохранении позиции точки')
            });
        });

        return group;
    }

    loadPoints() {
        $.get('/api/points', (points) => {
            points.forEach(p => {
                const group = this.createCircle(p);
                this.layer.add(group);
            });
            this.layer.draw();
        });
    }

    addComment() {
        if (!this.selectedPoint) return;
        this.selectedComment = null;
        this.showCommentForm();
        $('#context-menu').hide();
    }

    showEditForm() {
        if (!this.selectedPoint) return;

        this.isEditing = true;

        $('#point-x').val(this.selectedPoint.x);
        $('#point-y').val(this.selectedPoint.y);
        $('#point-color').val(this.selectedPoint.color);
        $('#point-radius').val(this.selectedPoint.radius);

        $('#point-form-modal').fadeIn();
        $('#context-menu').hide();
    }
}

$(document).ready(() => {
    window.app = new PointApp();
    app.init();
});
