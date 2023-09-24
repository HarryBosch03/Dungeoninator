extends Node2D

@export var panBaseSpeed = 500.0;
@export var panAcceleration = 150.0;
@export var moveAcceleration = 0.6;

@export var zoomSensitivity = 10.0
@export var zoomAcceleration = 0.1

@onready var camera = $Camera

var velocity = Vector2.ZERO
var panSpeed = 0.0
var zoom = 1.0
var zoomVelocity = 0.0
var zoomInput = 0.0

func _ready():
	pass # Replace with function body.
	
func _physics_process(delta):
	var input = Input.get_vector("moveLeft", "moveRight", "moveUp", "moveDown");
	
	if input.length() > 0.1:
		panSpeed += panAcceleration * delta;
	else:
		panSpeed = panBaseSpeed;
	
	var target = input * panSpeed / camera.zoom;
	var difference = target - velocity;
	var force = difference * 2.0 / moveAcceleration;
	
	position += velocity * delta;
	velocity += force * delta;
	
	var zoomForce = (zoomInput * zoomSensitivity - zoomVelocity) * 2.0 / zoomAcceleration;
	
	zoom += zoomVelocity * delta;
	zoomVelocity += zoomForce * delta;
	
	camera.zoom = Vector2.ONE * exp(zoom);
	zoomInput = 0.0;

func _input(event):
	if event is InputEventMouseButton and event.pressed:
		if event.button_index == MOUSE_BUTTON_WHEEL_UP:
			zoomInput += 1;
		if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
			zoomInput -= 1;
