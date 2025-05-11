import { usePermission } from "./usePermission";


export const Permission = () => {
    const {permissions} = usePermission();
    console.log('datos', permissions)
    return (
        <div>
            <div className="pl-8 space-y-2 text-sm">
                <label className="flex items-start gap-2">
                    <input type="checkbox" checked />
                    <span>
                        Gestión de Clientes <span className="text-gray-400">(GESTION_CLIENTES)</span><br />
                        <span className="text-gray-300">Descripción del permiso</span>
                    </span>
                </label>
                <label className="flex items-center gap-2">
                    <input type="checkbox" checked />
                    Gestión de Funcionarios
                </label>

                <label className="flex items-center gap-2">
                    <input type="checkbox" checked />
                    Gestión de Pacientes
                </label>
                <label className="flex items-center gap-2">
                    <input type="checkbox" checked />
                    Gestión de Cortes
                </label>
            </div>

            <div>
                <div className="pl-8 space-y-2 text-sm">
                    <label className="flex items-center gap-2">
                        <input type="checkbox" checked />
                        Gestión de Clientes
                    </label>
                    <label className="flex items-center gap-2">
                        <input type="checkbox" checked />
                        Gestión de Funcionarios
                    </label>
                    <label className="flex items-center gap-2">
                        <input type="checkbox" checked />
                        Gestión de Pacientes
                    </label>
                </div>
            </div>
        </div>
    )
}