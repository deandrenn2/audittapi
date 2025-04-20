import { ButtonPlay } from "../../../shared/components/Buttons/ButtonPlay";
import { ButtonPlus } from "../../../shared/components/Buttons/ButtonMas";
import { LinkSettings } from "../../Dashboard/LinkSenttings";

export const Roles = () => {
    return (
        <div className="p-6">
            <div className="">
                <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                    <LinkSettings />
                </div>
            </div>

            <div>
                <input type="text" placeholder="Crear" className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500
                 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2 mb-2"  />
                <button className="bg-indigo-500 hover:bg-indigo-900 text-white px-4 py-2 rounded-lg font-semibold">Crear Roles</button>
            </div>

            <div className="w-4xl border rounded p-4 mb-4">
                <div className="flex items-center mb-2 mr-2">
                    <div className="flex items-center gap-2">
                        <div>
                            <ButtonPlay />
                        </div>
                        <input value="SUPERADMIN" readOnly className="border rounded px-3 py-1 mr-2" />
                    </div>

                    <div className="flex items-center gap-2 " >
                        <ButtonPlus />
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

            <div className="w-4xl border rounded p-4">
                <div className="flex items-center mb-2">
                    <div className="flex items-center gap-2 mr-2">
                        <div>
                            <ButtonPlay />
                        </div>
                        <input value="ESTANDAR" readOnly className="border rounded px-3 py-1" />
                    </div>
                    <ButtonPlus />
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