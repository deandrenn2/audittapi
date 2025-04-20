import { useRef } from "react";
import { ButtonPlus } from "../../../shared/components/Buttons/ButtonMas";
import { ButtonPlay } from "../../../shared/components/Buttons/ButtonPlay";
import { LinkSettings } from "../../Dashboard/LinkSenttings";
import { useScales } from "./useScales";

export const Scales = () => {
    const { scales, createScale } = useScales();
    const refForm = useRef<HTMLFormElement>(null);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.target as HTMLFormElement;
        const formData = new FormData(form);
        const name = formData.get("name")?.toString().trim();

        if (!name) return;

        const response = await createScale.mutateAsync({ name });

        if (response.isSuccess) {
            refForm.current?.reset();
        }
    };

    return (
        <div className="p-6">
            <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                <LinkSettings />
            </div>

            <form onSubmit={handleSubmit} ref={refForm} className="mb-4">
                <input
                    type="text"
                    name="name"
                    placeholder="Crear la escala"
                    className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2" />
                <button
                    type="submit"
                    className="bg-indigo-500 hover:bg-indigo-900 text-white px-4 py-2 rounded-lg font-semibold">
                    Crear Escala
                </button>
            </form>

            {scales?.map((scale) => (
                <div key={scale.id} className="w-4xl p-4 mb-4 border rounded-lg shadow">
                    <div className="flex items-center mb-2 mr-2">
                        <div className="flex items-center gap-2">
                            <ButtonPlay />
                            <input
                                value={scale.name}
                                readOnly
                                className="border rounded px-3 py-1 mr-2"
                            />
                        </div>
                        <ButtonPlus />
                    </div>

                    <div className="pl-8 space-y-2 text-sm font-bold">
                        <label className="flex items-start gap-2 mb-2">
                            Cumple
                            <span className="text-red-500">Value: 1</span>
                        </label>
                        <label className="flex items-start gap-2 mb-2">
                            No Cumple
                            <span className="text-red-500">Value: 2</span>
                        </label>
                        <label className="flex items-start gap-2 mb-2">
                            No Aplica
                            <span className="text-red-500">Value: 0</span>
                        </label>
                    </div>
                </div>
            ))}
        </div>
    );
};
