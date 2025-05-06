export const Management = () => {
  
    return (
        <div>
            <div className="pl-8 space-y-2 text-sm">
                <label className="flex items-start gap-2">
                    <input type="checkbox" checked/>
                    <span>
                        Gestión de Clientes <span className="text-gray-400">(GESTION_CLIENTES)</span><br/>
                    <span className="text-gray-300">Descripción del permiso</span>
                    </span>
                </label>
                <label className="flex items-center gap-2">
                    <input type="checkbox" checked/>
                    Gestión de Funcionarios
                </label>

                <label className="flex items-center gap-2">
                    <input type="checkbox" checked/>
                        Gestión de Pacientes
                    </label>
                <label className="flex items-center gap-2">
                    <input type="checkbox" checked/>
                    Gestión de Cortes
                </label>
            </div>

            <div className="w-4xl border rounded p-4">
                <div className="flex items-center mb-2">
                    <div className="flex items-center gap-2 mr-2">
                        <div>

                        </div>
                        <input value="ESTANDAR" readOnly className="border rounded px-3 py-1" />
                    </div>
                </div>

                <div className="pl-8 space-y-2 text-sm">
                    <label className="flex items-center gap-2">
                        <input type="checkbox" checked/>
                        Gestión de Clientes
                    </label>
                    <label className="flex items-center gap-2">
                        <input type="checkbox" checked/>
                        Gestión de Funcionarios
                    </label>
                    <label className="flex items-center gap-2">
                        <input type="checkbox" checked/>
                        Gestión de Pacientes
                    </label>
                </div>
            </div>
        </div>
    )
}
