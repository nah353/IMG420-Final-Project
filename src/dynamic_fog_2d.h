#ifndef DYNAMIC_FOG_2D_H
#define DYNAMIC_FOG_2D_H

#include <godot_cpp/classes/node2d.hpp>
#include <godot_cpp/core/binder_common.hpp>

using namespace godot;

class DynamicFog2D : public Node2D {
    GDCLASS(DynamicFog2D, Node2D);

protected:
    static void _bind_methods();

public:
    DynamicFog2D();
    ~DynamicFog2D();

    void _ready() override;
};

#endif // DYNAMIC_FOG_2D_H