#include "dynamic_fog_2d.h"

#include <godot_cpp/variant/utility_functions.hpp>

using namespace godot;

void DynamicFog2D::_bind_methods() {
    // to be written
}

DynamicFog2D::DynamicFog2D() {
}

DynamicFog2D::~DynamicFog2D() {
}

void DynamicFog2D::_ready() {
    UtilityFunctions::print("DynamicFog2D loaded successfully");
}
