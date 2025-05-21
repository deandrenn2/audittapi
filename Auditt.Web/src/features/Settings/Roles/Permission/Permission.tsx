import Swal from "sweetalert2";
import ButtonDeletes from "../../../../shared/components/Buttons/ButtonDeletes";
import { usePermission } from "./usePermission";
import ButtonUpdates from "../../../../shared/components/Buttons/ButtonUpdates";
import { useState } from "react";
import { Direction } from "../../../../shared/components/OffCanvas/Models";
import OffCanvas from "../../../../shared/components/OffCanvas/Index";
import { permissionsModel } from "./PermissionModel";
import { PermisssionUpdate } from "./PermisssionUpdate";

export const Permission = () => {
    const { permissions, deletePermission } = usePermission();
    const [visible, setVsivible] = useState(false);
    const [visibleUpdate, setVisibleUpdate] = useState<permissionsModel | null>(null);

    const handleClose = () => {
        setVsivible(false);
        setVisibleUpdate(null);
    }

    const handleOpenEdit = (permissions: permissionsModel) => {
        setVisibleUpdate(permissions);
        setVsivible(true);
    }

    function handleDelete(e: React.MouseEvent<HTMLButtonElement>, id: number): void {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro de eliminar este permiso?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {
                await deletePermission.mutateAsync(id);
            }
        })
    }

    return (
        <div className="pl-8 space-y-2">
            {permissions && permissions.length > 0 ? (
                permissions?.map((perm) => (
                    <div key={String(perm.id || 'default-key')} className="flex items-start gap-2">
                        <input type="checkbox" checked readOnly />
                        <div >
                            {perm.name}{" "}
                            <span className="text-gray-400 font-bold">({perm.code})</span>
                            <br />
                            <div className="flex">
                                <span className="text-gray-400 mr-4">
                                    {perm.description || "Sin descripción"}
                                </span>
                                <div onClick={() => handleOpenEdit(perm)}>
                                    <ButtonUpdates />
                                </div>
                                <div className="flex relative ">
                                    {perm.id !== undefined && (
                                        <ButtonDeletes id={perm.id} onDelete={handleDelete} />
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>
                ))
            ) : (
                <p className="text-gray-400 font-bold">No hay permisos disponibles.</p>
            )}
            <OffCanvas
                titlePrincipal="Actualizar el permiso" visible={visible} xClose={handleClose} position={Direction.Right}>
                {visibleUpdate && <PermisssionUpdate data={visibleUpdate} />}
            </OffCanvas>
        </div>
    )
}