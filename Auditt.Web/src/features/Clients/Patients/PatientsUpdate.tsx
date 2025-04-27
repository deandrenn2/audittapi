import { useEffect, useRef, useState } from "react";
import { usePatients } from "./UsePatients";
import { PatientsModel } from "./PantientsModel";

export const PatientsUpdate = ({ data }: { data: PatientsModel }) => {
    const { updatePatients } = usePatients();
    const [patient, setPatient] = useState<PatientsModel>(data);
    const refForm = useRef<HTMLFormElement>(null);

    useEffect(() => {
        if (data) {
            setPatient(data);
        }
    }, [data, setPatient]);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const response = await updatePatients.mutateAsync(patient);

        if (response.isSuccess) {
            refForm.current?.reset();
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setPatient({ ...patient, [e.target.name]: e.target.value });
    };

    return (
        <div>
            <form ref={refForm} className="space-y-4" onSubmit={handleSubmit}>
                <div>
                    <label className="block text-sm font-medium mb-1">Nombre</label>
                    <input
                        type="text"
                        name="firstName"
                        value={patient.firstName}

                        className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200
                         hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2
                         focus:ring-indigo-400"
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Apellido</label>
                    <input
                        type="text"
                        name="lastName"
                        value={patient.lastName}

                        className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Número de Documento</label>
                    <input
                        type="text"
                        name="identification"
                        value={patient.identification}
                        required
                        className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium mb-1">Fecha de Nacimiento</label>
                    <input
                        type="text"
                        name="birthDate"
                        value={patient.birthDate}
                        required
                        className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">NIT</label>
                    <input
                        type="text"
                        name="eps"
                        value={patient?.eps}
                        required
                        className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <button
                        type="submit"
                        className="bg-[#392F5A] hover:bg-indigo-900 text-white px-8 py-2 rounded-lg font-semibold">
                        {updatePatients.isPending ? "Actualizando..." : "Actualizar"}
                    </button>
                </div>
            </form>
        </div>
    );
};
