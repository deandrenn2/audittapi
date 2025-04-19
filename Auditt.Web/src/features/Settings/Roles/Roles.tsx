
export const Roles = () => {
    return (
        <div className="p-6">
            <div className="flex items-center gap-4 mb-6">
                <input type="text" placeholder="" className="border px-4 py-2 rounded w-full max-w-sm" />
                <button className="bg-purple-800 text-white px-6 py-2 rounded-full font-semibold">Crear</button>
            </div>

            <div className="border rounded p-4 mb-4">
                <div className="flex items-center justify-between mb-2">
                    <div className="flex items-center gap-2">
                        <div className="bg-sky-100 text-sky-500 rounded-full p-1">
                            <svg className="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                                <path fill-rule="evenodd" d="M6 4l8 6-8 6V4z" clip-rule="evenodd" />
                            </svg>
                        </div>
                        <input value="SUPERADMIN" readOnly className="border rounded px-3 py-1" />
                    </div>
                    <div className="flex items-center gap-2">
                        <button className="text-green-600 text-xl"></button>
                        <button className="text-red-400 text-xl">−</button>
                    </div>
                </div>
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
            </div>

            <div className="border rounded p-4">
                <div className="flex items-center justify-between mb-2">
                    <div className="flex items-center gap-2">
                        <div className="bg-sky-100 text-sky-500 rounded-full p-1">
                            <svg className="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                                <path fill-rule="evenodd" d="M6 4l8 6-8 6V4z" clip-rule="evenodd" />
                            </svg>
                        </div>
                        <input value="ESTANDAR" readOnly className="border rounded px-3 py-1" />
                    </div>
                    <button className="text-green-600 text-xl">+</button>
                </div>
                
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
    );
}