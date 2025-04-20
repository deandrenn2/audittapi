import { ButtonPlus } from "../../../shared/components/Buttons/ButtonMas";
import { ButtonPlay } from "../../../shared/components/Buttons/ButtonPlay";
import { LinkSettings } from "../../Dashboard/LinkSenttings";

export const Scales = () => {
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

            <div className="w-4xl  p-4 mb-4">
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

                <div className="pl-8 space-y-2 text-sm font-bold">
                    <label className="flex items-start gap-2 mb-2">
                        Cumple
                        <span className="text-red-500">Value: 1</span><br />

                    </label>

                    <label className="flex items-start gap-2 mb-2 font-bold">
                        No Cumple
                        <span className="text-red-500">Value: 2</span><br />

                    </label>

                    <label className="flex items-start gap-2 mb-2 font-bold">
                         No Aplica
                        <span className="text-red-500">Value: 0</span><br />
                    </label>
                </div>
            </div>

            <div className="w-4xl  rounded p-4">
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
                    <label className="flex items-start gap-2 mb-2 font-bold">
                        Realizado
                        <span className="text-red-500">Value: 1</span><br />
                    </label>

                    <label className="flex items-start gap-2 mb-2 font-bold">
                        No Realizado
                        <span className="text-red-500">Value: 2</span><br />
                    </label>

                    <label className="flex items-start gap-2 mb-2 font-bold">
                         No Aplica 
                        <span className="text-red-500">Value: 0</span><br />
                    </label>
                </div>
            </div>
        </div>
    );
}