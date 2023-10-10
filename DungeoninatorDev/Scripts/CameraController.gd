extends Node2D

@export_category("Camera Controller")
@export_group("Pan")
@export var shiftSpeedIncrease : float = 2.0
@export var ctrlSpeedIncrease : float = 5.0

@export var panBaseSpeed : float = 500.0
@export var panAcceleration : float = 150.0
@export var moveAcceleration : float = 0.25

@export_group("Grab")
@export var grabSensitivity : float = 100.0
@export var grabSmoothing : float = 5.0

@export_group("Zoom")
@export var zoomSensitivity : float = 10.0
@export var zoomAcceleration : float = 0.1

@export_group("Background")
@export_range(0, 255) var backgroundOpacity : int = 8

@onready var camera : Camera2D = $Camera
@onready var background : Sprite2D = $Camera/Background

var velocity : Vector2 = Vector2.ZERO
var force : Vector2 = Vector2.ZERO
var panSpeed : float = 0.0
var zoom : float = 1.0
var zoomVelocity : float = 0.0
var zoomForce : float = 0.0
var zoomInput : float = 0.0
var lastMousePos : Vector2
var velocityScale : float = 1.0

func _ready():
	background.modulate.a = backgroundOpacity / 255.0

func _process(delta):
	background.region_rect.size = camera.get_viewport_rect().size / camera.zoom

func _physics_process(delta : float):
	
	velocityScale = 1.0;
	if Input.is_key_pressed(KEY_SHIFT):
		velocityScale *= shiftSpeedIncrease
	if Input.is_key_pressed(KEY_CTRL):
		velocityScale *= ctrlSpeedIncrease
		
	velocity /= velocityScale
	
	keyboardInput(delta)
	mouseInput(delta)
	
	velocity *= velocityScale
	
	itterate(delta)
	
	camera.zoom = Vector2.ONE * exp(zoom)
	zoomInput = 0.0


func keyboardInput(delta : float):
	var input : Vector2 = Input.get_vector("moveLeft", "moveRight", "moveUp", "moveDown")
	
	if input.length() > 0.1:
		panSpeed += panAcceleration * delta
	else:
		panSpeed = panBaseSpeed
	
	var target : Vector2 = input * panSpeed / camera.zoom
	var difference : Vector2 = target - velocity
	force += difference * 2.0 / moveAcceleration


func mouseInput(delta : float):
	
	var mousePos : Vector2 = get_viewport().get_mouse_position()
	var mouseDelta : Vector2 = mousePos - lastMousePos
	var target : Vector2 = -mouseDelta * grabSensitivity / camera.zoom.y
		
	if Input.is_mouse_button_pressed(MOUSE_BUTTON_RIGHT):
		force += (target - velocity) * grabSmoothing
		
	lastMousePos = mousePos;
	
	zoomForce = (zoomInput * zoomSensitivity - zoomVelocity) * 2.0 / zoomAcceleration


func itterate(delta : float):
	position += velocity * delta
	velocity += force * delta
	force = Vector2.ZERO
	
	zoom += zoomVelocity * delta
	zoomVelocity += zoomForce * delta
	zoomForce = 0.0


func _input(event):
	if event is InputEventMouseButton and event.pressed:
		if event.button_index == MOUSE_BUTTON_WHEEL_UP:
			zoomInput += 1
		if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
			zoomInput -= 1
